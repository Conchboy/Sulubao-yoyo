using System;
using System.IO;
using Core.Base;

namespace TestFrequencyAdjustment
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("测试码表调频功能");
            
            // 创建InputMode实例
            InputMode inputMode = new InputMode();
            
            // 设置MasterDitPath
            inputMode.MasterDitPath = "../../../dict/麓鸣/MasterDit.shp";
            
            // 加载码表
                if (File.Exists(inputMode.MasterDitPath))
                {
                    inputMode.MasterDit = File.ReadAllLines(inputMode.MasterDitPath, System.Text.Encoding.UTF8);
                    inputMode.UserDit = new string[0]; // 初始化UserDit数组
                    inputMode.indexComplete = true;
                    
                    // 创建索引
                    inputMode.CreateIndex(inputMode.MasterDit, ref inputMode.DictIndex.IndexList, 1, 0, inputMode.MasterDit.Length);
                
                Console.WriteLine("码表加载成功，共" + inputMode.MasterDit.Length + "条记录");
                
                // 测试输入"ff" (两码编码)
                string input = "ff";
                Console.WriteLine("\n测试输入: " + input);
                
                // 获取输入值
                string[] results = inputMode.GetInputValue(input);
                if (results != null)
                {
                    Console.WriteLine("候选词:");
                    for (int i = 0; i < results.Length; i++)
                    {
                        string[] parts = results[i].Split('|');
                        Console.WriteLine((i + 1) + ". " + parts[1]);
                    }
                    
                    // 选择第2个候选词进行调频
                    int targetPos = 2;
                    Console.WriteLine("\n选择第" + targetPos + "个候选词进行调频");
                    
                    bool success = inputMode.UpdatePos(input, targetPos);
                    if (success)
                    {
                        Console.WriteLine("调频成功！");
                        
                        // 再次获取输入值，验证调频效果
                        Console.WriteLine("\n调频后的候选词:");
                        results = inputMode.GetInputValue(input);
                        if (results != null)
                        {
                            for (int i = 0; i < results.Length; i++)
                            {
                                string[] parts = results[i].Split('|');
                                Console.WriteLine((i + 1) + ". " + parts[1]);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("调频失败！");
                    }
                }
                else
                {
                    Console.WriteLine("未找到匹配的候选词");
                }
            }
            else
            {
                Console.WriteLine("码表文件不存在");
            }
            
            Console.WriteLine("\n测试完成，按任意键退出...");
            Console.ReadKey();
        }
    }
}