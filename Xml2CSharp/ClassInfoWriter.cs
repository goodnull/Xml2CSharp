using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xml2CSharp
{
    public class ClassInfoWriter
    {
        private readonly IEnumerable<Class> _classInfo;

        public ClassInfoWriter(IEnumerable<Class> classInfo)
        {
            _classInfo = classInfo;
        }

        public void Write(TextWriter textWriter)
        {
            using (textWriter)
            {
                textWriter.WriteLine("using System.Xml.Serialization;");
                textWriter.WriteLine("");
                textWriter.WriteLine("");
                foreach (var @class in _classInfo.Reverse())
                {
                    textWriter.Write($"[XmlRoot(ElementName=\"{@class.XmlName}\"");
                    textWriter.WriteLine(string.IsNullOrWhiteSpace(@class.Namespace) ? ")]" : $",Namespace=\"{@class.Namespace}\")]");
                    textWriter.WriteLine("public class {0} {{", @class.Name);
                    textWriter.WriteLine("");
                    foreach (var field in @class.Fields)
                    {
                        textWriter.Write($"\t[Xml{field.XmlType}({field.XmlType}Name=\"{field.XmlName}\"");
                        textWriter.WriteLine(string.IsNullOrWhiteSpace(field.Namespace) ? ")]" : $",Namespace=\"{field.Namespace}\")]");
                        if (field.Type.ToLower() == "string")
                            field.Type = field.Type.ToLower();
                        textWriter.WriteLine($"\tpublic {field.Type} {field.Name}{{ get; set; }}");
                        textWriter.WriteLine("");
                    }
                    textWriter.WriteLine("}");
                    textWriter.WriteLine("");
                }
            }
        }


    }
} 