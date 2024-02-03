namespace MyBlog.DAL.Implementations
{
    public static class DbHelper
    {
        public static string GetConnectionString()
        {
            // строока подключения прописана жестко только в тестовых целях
            // в реальном приложении информация должна загружаться из конфигурации
            return "Data Source=.;Initial Catalog=hackishssharp;uid=sa;pwd=Hujkq23&6#;Trust Server Certificate=true;";
        }
    }
}

