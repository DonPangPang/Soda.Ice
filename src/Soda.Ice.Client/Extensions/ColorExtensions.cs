namespace Soda.Ice.Client.Extensions
{
    public static class ColorExtensions
    {
        private static string[] Colors = new[] { "#EF5350", "#EC407A", "#AB47BC",
        "#7E57C2","#5C6BC0","#42A5F5",
        "#81D4FA","#26C6DA","#26A69A",
        "#66BB6A","#9CCC65","#EC407A",
        "#FF7043","#8D6E63","#78909C"};

        private static Random Random = new Random();

        public static string GetRandomColor()
        {
            return Colors[Random.Next(Colors.Length - 1)];
        }
    }
}