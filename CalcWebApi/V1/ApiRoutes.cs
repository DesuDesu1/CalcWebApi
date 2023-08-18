namespace CalcWebApi.V1
{
    public static class ApiRoutes
    {
        public const string Root = "calcapi";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class MathOperations
        {
            public const string Multiply = Base + "/multiply/";
            public const string Divide = Base + "/divide/";
            public const string Pow = Base + "/pow/";
            public const string Root = Base + "/root/";
            public const string Addition = Base + "/add/";
            public const string Subtraction = Base + "/subtract/";
            public const string ExpressionEvaluation = Base + "/evaluate/";
        }
    }

}
