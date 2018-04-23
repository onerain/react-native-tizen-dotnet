namespace ReactNative.Modules.Image
{
    struct ImageMetadata
    {
        public ImageMetadata(string uri, double width, double height)
        {
            Uri = uri;
            Width = width;
            Height = height;
        }

        public string Uri { get; }

        //public int Width { get; }

        //public int Height { get; }

        public double Width { get; }

        public double Height { get; }
    }
}
