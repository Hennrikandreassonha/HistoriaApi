using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistoriaApi.Service
{
    public class FileService
    {
        public AiService AiService { get; set; }
        public FileService()
        {
            AiService = new AiService(File.ReadAllLines("../../HistoryEmailDocs/openaiapikey.txt")[0]);
        }
        public void AddSubject(string filePath, HistoryInput subject)
        {
            var file = File.ReadAllLines(filePath).ToList();
            file.Add($"{subject.Subject} - {(string.IsNullOrEmpty(subject.User) ? "Ingen angiven användare" : subject.User)}");
            File.WriteAllLines(filePath, file);
        }
        public bool IsHistorySubject(string subject){
            return AiService.SendQuestion(subject).Result == "Yes";
        }
    }
    public class HistoryInput
    {
        public string? User { get; set; }
        public string Subject { get; set; }
    }
}