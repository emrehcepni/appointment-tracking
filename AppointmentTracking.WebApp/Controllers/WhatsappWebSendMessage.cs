using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

public class WhatsAppService
{
    private readonly string phoneNumber = "05439246411"; // Burada telefon numarasını sabit veriyoruz

    public string SendMessage(string message)
    {
        // Chrome kullanıcı profili yolunu belirt
        string userDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google", "Chrome", "User Data");
        string profileDir = "Default"; // Varsayılan profil

        ChromeOptions options = new ChromeOptions();
        options.AddArgument($"--user-data-dir={userDataDir}");
        options.AddArgument($"--profile-directory={profileDir}");

        try
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {
                // WhatsApp Web'e git
                driver.Navigate().GoToUrl("https://web.whatsapp.com");

                Console.WriteLine("Lütfen QR kodunu tarayın ve giriş yapın...");

                // Kullanıcının giriş yapmasını bekle
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                wait.Until(d => d.FindElement(By.XPath("//div[@contenteditable='true'][@data-tab='3']")));

                // Telefon numarasını arama kutusuna yaz
                var searchBox = driver.FindElement(By.XPath("//div[@contenteditable='true'][@data-tab='3']"));
                searchBox.SendKeys(phoneNumber);
                System.Threading.Thread.Sleep(2000);
                searchBox.SendKeys(Keys.Enter);

                // Mesaj kutusunu bul
                wait.Until(d => d.FindElement(By.XPath("//div[@contenteditable='true'][@data-tab='10']")));
                var messageBox = driver.FindElement(By.XPath("//div[@contenteditable='true'][@data-tab='10']"));
                messageBox.SendKeys(message);
                System.Threading.Thread.Sleep(20000);
                messageBox.SendKeys(Keys.Enter);

                Console.WriteLine("Mesaj gönderildi!");
                return "Mesaj başarıyla gönderildi.";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata oluştu: {ex.Message}");
            return $"Hata oluştu: {ex.Message}";
        }
    }
}
