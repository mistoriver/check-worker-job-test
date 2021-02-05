using CheckWatcher.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace CheckWatcher
{
    public partial class WatcherService : ServiceBase
    {
        Watcher watcher;
        public WatcherService()
        {
            InitializeComponent();
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            watcher = new Watcher();
            Task.Run(() => watcher.Start());
        }

        protected override void OnStop()
        {
            watcher.Stop();
            Thread.Sleep(1000);
        }
    }

    class Watcher
    {
        FileSystemWatcher fileWatcher;
        bool enabled;
        object locker = new object();

        public Watcher()
        {
            ServiceLogger.InitLogger();
            var folderToWatch = ((NameValueCollection)ConfigurationManager.GetSection("ApplicationSettings"))["CheckFolder"];
            ServiceLogger.Log.Info($"Для наблюдения задана папка {folderToWatch}");
            FileHelper.TryCreateFolder(folderToWatch);
            fileWatcher = new FileSystemWatcher(folderToWatch);
            fileWatcher.Changed += Watcher_ChangedOrCreated;
            fileWatcher.Created += Watcher_ChangedOrCreated;
        }
        public void Start()
        {
            enabled = true;
            fileWatcher.EnableRaisingEvents = true;
            ServiceLogger.Log.Info("Служба запущена");
            while (enabled)
            {
                Thread.Sleep(1000);
            }
        }
        public void Stop()
        {
            fileWatcher.EnableRaisingEvents = false;
            enabled = false;
            ServiceLogger.Log.Info("Служба остановлена");
        }

        private void Watcher_ChangedOrCreated(object sender, FileSystemEventArgs e)
        {
            string filePath = e.FullPath;
            if (filePath.EndsWith(".txt"))
            {
                lock (locker)
                {
                    if (FileHelper.IsFileExists(filePath))
                    {
                        ServiceLogger.Log.Info("Файл замечен");
                        var textFromFile = FileHelper.GetTextFromFile(filePath);
                        var cheque = JsonHelper.DeserializeJson(textFromFile);
                        if (cheque != null)
                        {
                            ServiceLogger.Log.Info("Попытка отправить файл в клиент");
                            DbProxyReference.DbProxyClient client = new DbProxyReference.DbProxyClient();
                            try
                            {
                                client.TakeCheque(cheque);
                                client.Close();
                                ServiceLogger.Log.Info("Файл успешно отправлен в клиент");
                            }
                            catch (Exception ex)
                            {
                                client.Abort();
                                ServiceLogger.Log
                                    .Error($"Произошла ошибка при попытке отправить файл в клиент: {Environment.NewLine + ex}");
                            }
                            FileHelper.DeleteFile(filePath);
                        }
                    }
                }
            }
        }
    }
}
