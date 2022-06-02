using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OneRegister.Web.Models.Audit;
using OneRegister.Web.Models.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OneRegister.Web.Services.Audit
{
    public class AuditService
    {
        private readonly IConfiguration _configuration;
        public AuditService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<SelectListItem> GetExistLogFileListItems()
        {
            var logPath = GetLogPath();
            var files = Directory.GetFiles(logPath)
                .Where(f => f.Contains(".log")
                && f.Contains("OneRegister"))
                .OrderByDescending(f => f);
            if (files.Any())
            {
                return files.Select(f => new SelectListItem { Text = Path.GetFileNameWithoutExtension(f), Value = Path.GetFileNameWithoutExtension(f) }).ToList();
            }

            return new List<SelectListItem> { new SelectListItem { Text = "NoLogFileFound", Value = "" } };
        }
        private StringBuilder stringBuilder;
        private List<LogDataGrid> result;
        internal List<LogDataGrid> GetLogFile(string fileName)
        {
            var logPath = GetLogPath();
            var filePath = Directory.GetFiles(logPath, fileName + "*");
            if (!string.IsNullOrEmpty(filePath.FirstOrDefault()))
            {
                //TODO check file size and block big files
                using var fs = new FileStream(path: filePath.First(), mode: FileMode.Open, access: FileAccess.Read, share: FileShare.ReadWrite);
                using var reader = new StreamReader(fs);
                var text = reader.ReadToEnd();
                var lines = text.Split(Environment.NewLine);
                stringBuilder = new StringBuilder();
                result = new List<LogDataGrid>();
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    AddlineToList(line);
                }
            }

            return result;
        }

        private void AddlineToList(string line)
        {
            if (line.StartsWith("20"))
            {
                if(stringBuilder.Length > 0)
                {
                    result.Add(new LogDataGrid
                    {
                        Time = string.Empty,
                        Level = string.Empty,
                        Message = stringBuilder.ToString()
                    });
                    stringBuilder.Clear();
                }
                var sections = line.Split("|");
                if (sections.Length == 6)
                {
                    result.Add(new LogDataGrid
                    {
                        Time = sections[0],
                        Level = sections[1],
                        Session = sections[2],
                        RemoteAddress = sections[3],
                        User = sections[4],
                        Message = sections[5]
                    });
                }
                else
                {
                    result.Add(new LogDataGrid
                    {
                        Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        Level = "ParseError",
                        Message = line
                    });
                }
            }
            else
            {
                stringBuilder.AppendLine(line);
            }
        }

        private string GetLogPath()
        {
            var config = _configuration.GetSection("Services:Serilog").Get<SerilogConfigModel>();
            if (config is null)
            {
                throw new System.Exception("Log Configuration Section is not available in appsettings.json");
            }

            return config.Path;
        }

    }
}
