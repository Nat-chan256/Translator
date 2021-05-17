using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    // Класс, управляющий метками в программе
    class LabelsManager
    {
        // Количество меток в обрабатываемой программе
        private static int labelsCount;

        static LabelsManager()
        {
            labelsCount = 0;
        }
        public static string GetNewLabel()
        {
            return $"М{++labelsCount}";
        }

        // Сбрасывает значение переменной labelsCount
        public static void Reset()
        {
            labelsCount = 0;
        }
    }
}
