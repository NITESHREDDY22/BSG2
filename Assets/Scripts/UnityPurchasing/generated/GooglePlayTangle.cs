// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("beMt3XAIwzzlWdDfeV0HT9IAtsUlR+98RA+6+TCDFh0DqIzZfF+emdEtlw/zM8GYUWay/bXNDkftzb1EgJUTwdM3fTSLkxfYS2DSi2I8oKw4ZiP3Od5ToYL5XfZ63hU4N9TCPhBxQjzpdfyqKXF/vQ5oW1fYqpHQSft4W0l0f3BT/zH/jnR4eHh8eXr7eHZ5Sft4c3v7eHh54sbKfsVJL8csinOx4NiRZOjMfqcoJOyqsA2oZbxCJFqsPTsHIp/JsnDFq45VmZV3SIKHZgUiwkCJzn12PHzqTYOY79BE+ftIh8pGQZnXdZ5pgAC02uWSWzpZN7HPBzm4UJtflhyBqX67TnXsDYKNWI8M+evONQk+IXCff5ngDkqtyZegQnrgtnt6eHl4");
        private static int[] order = new int[] { 12,2,8,3,5,7,12,8,12,11,10,11,13,13,14 };
        private static int key = 121;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
