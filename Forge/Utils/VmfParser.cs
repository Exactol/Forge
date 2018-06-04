using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Sprache;

namespace Forge
{
    public class VmfParser
    {
        private TextReader _reader = new StreamReader("cube.vmf");

        public void ParseAndPrint()
        {
            var vmfInput = Grammer.Vmf.Parse(_reader.ReadToEnd());

            foreach (var section in vmfInput.Sections)
            {
                Console.WriteLine($"VmfClass Name: {section.Name}");
                Console.WriteLine("Properties:");
                foreach (var property in section.Properties)
                {
                    Console.Write(property.Name + " ");
                    Console.WriteLine(property.Value);
                }
                Console.WriteLine("\n");
            }

            //var vmfInput = Grammer.VmfClass.Parse(_reader.ReadToEnd());
            //Console.WriteLine(vmfInput.Name);
            //foreach (var property in vmfInput.Properties)
            //{
            //    Console.Write(property.Name);
            //    Console.WriteLine(property.Value);
            //}


        }

    }

    public enum PropetyType
    {
        None,
        Int,
        String,
        Uri,
        Material,
        UV,
        Plane
    }
    public class Property
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public Section Section { get; private set; }

        public Property(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public Property(string name, Section section)
        {
            Name = name;
            Section = section;
        }
    }

    public class Section
    {
        public string Name { get; private set; }
        public IEnumerable<Property> Properties { get; private set; }

        public Section(string name, IEnumerable<Property> properties)
        {
            Name = name;
            Properties = properties;
        }
    }

    //public class Side
    //{
    //    public int Id { get; private set; }
    //    public Vector3 plane { get; private set; }
    //    public string Material { get; private set; }
    //    public Tuple<Vector4, float> UAxis { get; private set; }
    //    public Tuple<Vector4, float> VAxis { get; private set; }
    //    public float Rotation { get; private set; }
    //    public float LightMapScale { get; private set; }
    //    public int SmoothingGroup { get; private set; }

    //    public Side(string id, string plane, string material, string uAxis, string vAxis, string Rotation, string lightMapScale, string smoothingGroup)
    //    {
    //        Id = int.Parse(id);

    //    }
    //}


    static class Grammer
    {
        private static readonly Parser<char> DoubleQuote = Parse.Char('"').Token();
        private static readonly Parser<char> CurlyL = Parse.Char('{').Token();
        private static readonly Parser<char> CurlyR = Parse.Char('}').Token();

        private static readonly Parser<string> QuotedText =
            (from open in DoubleQuote
            from content in Parse.CharExcept('"').Many().Text()
            from close in DoubleQuote
            select content).Token();

        private static readonly Parser<string> Identifier =
            QuotedText.Token();

        private static readonly Parser<string> ExternIdentifier = Parse.Letter.Many().Text().Token();

        public static readonly Parser<Section> InnerVmfClass =
            from name in ExternIdentifier
            from first in CurlyL
            from properties in Property.Many()
            from last in CurlyR
            select new Section(name, properties);

        private static readonly Parser<Property> Property =
        (from name in Identifier
            from value in QuotedText
            select new Property(name, value));

        //public static readonly Parser<VmfValue> vmfValue =
        //    Parse.Ref(() => InnerVmfClass)
        //        .Or(Parse.Ref(() => Property));


        public static readonly Parser<Section> VmfClass =
            from name in ExternIdentifier
            from first in CurlyL
            from properties in Property.Many()
            from last in CurlyR
            select new Section(name, properties);
        
        public static readonly Parser<Vmf> Vmf =
            from sections in VmfClass.Many()
            select new Vmf(sections);
        //VmfClass.Many().Select(sections => new Vmf(sections)).End();
        

    }

    public class Vmf
    {
        public IEnumerable<Section> Sections { get; private set; }

        public Vmf(IEnumerable<Section> sections)
        {
            Sections = sections;
        }
    }

    public class VmfValue
    {
        
    }
    
}
