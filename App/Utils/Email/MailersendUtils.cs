using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ServeBooks.Models;

namespace ServeBooks.App.Utils.Email
{
    public class MailersendUtils
    {
        public async Task EnviarCorreo(string emailUser, string textSubject, string textBody)
        {
            try
            {
                string url = "https://api.mailersend.com/v1/email";
                string tokenEmail = "mlsn.9623d12db45cb715be2018c3697b0bdbdb0b1a09d1b43660e4240c90f5686541";

                // Construir el mensaje de correo electrónico
                var emailMessage = new
                {
                    from = new { email = "NanoDrive@trial-3vz9dlew96nlkj50.mlsender.net", name = "Your Name" },
                    to = new List<object>
                    {
                        new { email = emailUser, name = "Recipient Name" }
                    },
                    subject = textSubject,
                    text = textBody
                };

                // Serializar el objeto email en formato JSON
                string jsonBody = JsonSerializer.Serialize(emailMessage);

                using (HttpClient client = new HttpClient())
                {
                    // Configurar el encabezado de Authorization para indicar el token de autorización
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenEmail);

                    // Crear el contenido de la solicitud POST como StringContent
                    StringContent stringContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    // Realizar la solicitud POST a la URL indicada
                    HttpResponseMessage response = await client.PostAsync(url, stringContent);

                    // Verificar si la solicitud fue exitosa (código de estado: 200 - 209)
                    if (response.IsSuccessStatusCode)
                    {
                        // Mostrar el estado de la solicitud
                        Console.WriteLine($"Correo electrónico enviado correctamente a: {emailUser}. Estado de la solicitud: {response.StatusCode}");
                    }
                    else
                    {
                        // Leer el contenido de la respuesta en caso de error para entender mejor el problema
                        string errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"El correo electrónico no pudo ser enviado a: {emailUser}. Estado de la solicitud: {response.StatusCode}. Detalles: {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error al enviar el correo electrónico. Detalles: {ex.Message}");
            }
        }
    }
}
