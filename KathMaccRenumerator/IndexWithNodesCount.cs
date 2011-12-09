//Created by Evgeniya Martynova
//Corsunina Core: Class-helper for Kathill-Macc Alghorithm

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Индекс точки-претендента на то, чтобы быть первой в перенумерации, с количеством уровней для неё 
namespace KathMaccRenumerator
{
    class IndexWithNodesCount
    {
        public int Index { get; set; }
        public int NodeCount { get; set; }

        public IndexWithNodesCount(int index, int nodeCount)
        {
            this.Index = index;
            this.NodeCount = nodeCount;
        }
    }
}
