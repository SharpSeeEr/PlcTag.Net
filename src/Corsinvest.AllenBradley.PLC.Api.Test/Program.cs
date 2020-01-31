using System;
using System.Collections.Generic;
using Corsinvest.AllenBradley.PLC.Api;
using Microsoft.Extensions.Logging;

namespace Corsinvest.AllenBradley.Test
{
    [Serializable]
    public class Test12
    {
        public int AA1 { get; set; }
        public int AA2 { get; set; }
        public int AA3 { get; set; }
        public int AA4 { get; set; }
        public int AA5 { get; set; }
        public int AA6 { get; set; }
        public int AA7 { get; set; }
        public int AA8 { get; set; }
    }

    public class Program
    {
        private static void PrintChange(string @event, OperationResult result)
        {
            Console.Out.WriteLine($"{@event} {result.Timestamp} Changed: {result.Tag.Name} {result.StatusCode}");
        }

        static void TagChanged(OperationResult result)
        {
            PrintChange("TagChanged", result);
        }
        static void GroupChanged(IEnumerable<OperationResult> results)
        {
            foreach (var result in results) PrintChange("GroupTagChanged", result);
        }

        public static void Main(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole();
            });
            ILogger<Controller> logger = loggerFactory.CreateLogger<Controller>();
            
            logger.LogInformation("Example log message");
            using (var controller = new Controller("10.155.128.192", "1, 0", CPUType.LGX, logger))
            {
                controller.Timeout = 1000;
                // controller.DebugLevel=3;
                Console.Out.WriteLine("Ping " + controller.Ping(true));

                var tagp12 = controller.CreateTag<Int64>("TKP_PC_B_P64");
                var tagp2 = controller.CreateTag<Int32>("TKP_PC_B_P2");
                tagp2.Read();
                tagp2.Value = 1;

                var tag12 = controller.CreateTag<Int32>("TKP_PLC_D_P1[10]");

                var tagBPLC1 = controller.CreateTag<Int32>("TKP_PLC_B_P1");
                tagBPLC1.Read();

                var tagOvenEnabled = controller.CreateTag<bool>("TKP_PLC_B_OVEN");
                var oven = tagOvenEnabled.Read();
                Console.Out.WriteLine(oven.Tag.Value);

                System.Threading.Thread.Sleep(800);

                Console.Out.WriteLine("pippo");

                oven = tagOvenEnabled.Read();
                Console.Out.WriteLine(oven.Tag.Value);


                // var tagBPC1 = grp.CreateTagInt32("TKP_PC_B_P1");
                // var tagBarcode = grp.CreateTagString("TKP_PLC_S_P1");

                // var tag3 = grp.CreateTagArray<float[]>("OvenTemp", 36);

                // //var tag_1 = grp.CreateTagArray<string[]>("Track", 300);

                //or
                var tag = controller.CreateTag<string>("Track");
                tag.Changed += TagChanged;
                var aa = tag.Read();

                Console.Out.WriteLine(aa);

                var tag1 = controller.CreateTag<Test12>("Test");
                tag.Changed += TagChanged;

                var tag2 = controller.CreateTag<float>("Fl32");


                //grp.Changed += GroupChanged;
                //grp.Read();

            }
        }
    }
}