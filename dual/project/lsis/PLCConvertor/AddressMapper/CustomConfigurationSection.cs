using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressMapper
{
    public enum CopyPolicy
    {
        DoNotOverwrite,
        OverwriteIfFileChanged,
        OverwriteAnyway,
    }

    public class CustomConfigurationSection : System.Configuration.ConfigurationSection
    {
        [ConfigurationProperty("allowOverwrite", DefaultValue = true)]
        public bool AllowOverwrite
        {
            get { return (bool)this["allowOverwrite"]; }
            set { this["allowOverwrite"] = value; }
        }


        [ConfigurationProperty("skipCopySameFileDateSize", DefaultValue = true)]
        public bool SkipCopySameFileDateSize
        {
            get { return (bool)this["skipCopySameFileDateSize"]; }
            set { this["skipCopySameFileDateSize"] = value; }
        }


        [ConfigurationProperty("fileOverwritePolicy", DefaultValue = CopyPolicy.OverwriteAnyway)]
        public CopyPolicy FileOverwritePolicy
        {
            get { return (CopyPolicy)this["fileOverwritePolicy"]; }
            set { this["fileOverwritePolicy"] = value; }
        }


        [ConfigurationProperty("sourceFolderPrefix")]
        public string SourceFolderPrefix
        {
            get { return (string)this["sourceFolderPrefix"]; }
            set { this["sourceFolderPrefix"] = value; }
        }

        //[ConfigurationProperty("destinationDBConnectionString")]
        //public string DestinationDBConnectionString
        //{
        //    get
        //    {
        //        return ((string)this["destinationDBConnectionString"]).NonNullEmptySelector(
        //            $"SERVER={DestinationDBServerIp};DATABASE={DestinationDBSchema};PORT=3306;USER=pdv;PASSWORD=pdv;COMPRESS=true;ALLOW USER VARIABLES=true;CONNECTIONTIMEOUT=300");
        //    }
        //    set { this["destinationDBConnectionString"] = value; }
        //}


        [ConfigurationProperty("destinationFolderPrefix")]
        public string DestinationFolderPrefix
        {
            get { return (string)this["destinationFolderPrefix"]; }
            set { this["destinationFolderPrefix"] = value; }
        }

        [ConfigurationProperty("destinationDBServerIp")]
        public string DestinationDBServerIp
        {
            get { return (string)this["destinationDBServerIp"]; }
            set { this["destinationDBServerIp"] = value; }
        }
    }
}
