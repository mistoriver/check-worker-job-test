using CheckWorker.Helpers;
using Newtonsoft.Json;
using System;

namespace CheckWatcher.Helpers
{
    public static class JsonHelper
    {
        public static Cheque DeserializeJson(string json)
        {
            Cheque cheque = null;
            try
            {
                cheque = JsonConvert.DeserializeObject<Cheque>(json);
                if (cheque != null)
                    ServiceLogger.Log.Info("Файл успешно десериализован");
                else
                    ServiceLogger.Log.Info("Произведена попытка десериализовать файл, но он был пуст");
            }
            catch (Exception e)
            {
                ServiceLogger.Log.Error($"Произошла ошибка при десериализации файла:{Environment.NewLine + e.ToString()}");
            }
            return cheque;
        }
    }
}
