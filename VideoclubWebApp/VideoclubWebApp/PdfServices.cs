using DinkToPdf;
using DinkToPdf.Contracts;
using System.Text;

namespace VideoclubWebApp.Services
{
    public interface IPdfService
    {
        byte[] GeneratePdf(string htmlContent);
    }

    public class PdfService : IPdfService
    {
        private readonly IConverter _converter;

        public PdfService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GeneratePdf(string htmlContent)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 },
                DocumentTitle = "Reporte VideoClub"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 9, Right = "Página [page] de [toPage]", Line = true },
                FooterSettings = { FontSize = 9, Line = true, Center = "VideoClub - Reporte generado el " + DateTime.Now.ToString("dd/MM/yyyy") }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return _converter.Convert(pdf);
        }
    }
}