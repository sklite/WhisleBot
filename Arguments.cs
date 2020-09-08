using CommandLine;

namespace WhisleBotConsole
{
    class Arguments
    {
        [Option('a', "assetsToProcess", Required = false, Default = 100, HelpText = "The number of assets to watermark")]
        public int AssetsToProcess { get; set; }

        [Option('s', "skipAssetsCount", Required = false, Default = 0, HelpText = "The number of assets to skip")]
        public int SkipAssetsCount { get; set; }

        [Option('b', "assetsBatchCount", Required = false, Default = 100, HelpText = "The number of assets to batch during the watermarking process")]
        public int AssetsBatchCount { get; set; }
    }
}
