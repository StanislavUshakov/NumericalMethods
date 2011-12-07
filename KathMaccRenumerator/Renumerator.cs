//Created by Evgenia Martynova
//Corsunina Core: Renumeration with Kathill-Macc Alghorithm

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace KathMaccRenumerator
{
    public static class Renumerator
    {
        //uncomment this lines to get Points for testing
        /*  private static void GetPointsForTest()
          {
              orderedPoints = new List<Point>();
              for (int i = 1; i <= 12; i++)
              {
                  Point point1 = new Point(0, 0);
                  point1.Index = i;
                  orderedPoints.Add(point1); 
              }

              starsForPoints = new List<List<int>>();

              List<int> star1 = new List<int>();
              star1.Add(2);
              star1.Add(8);
              star1.Add(9);
              star1.Add(12);
              starsForPoints.Add(star1);

              List<int> star2 = new List<int>();
              star2.Add(1);
              star2.Add(3);
              star2.Add(7);
              star2.Add(8);
              starsForPoints.Add(star2);

              List<int> star3 = new List<int>();
              star3.Add(4);
              star3.Add(2);
              star3.Add(6);
              star3.Add(7);
              starsForPoints.Add(star3);

              List<int> star4 = new List<int>();
              star4.Add(5);
              star4.Add(3);
              star4.Add(6);
              starsForPoints.Add(star4);

              List<int> star5 = new List<int>();
              star5.Add(4);
              star5.Add(6);
              starsForPoints.Add(star5);

              List<int> star6 = new List<int>();
              star6.Add(5);
              star6.Add(4);
              star6.Add(3);
              star6.Add(7);
              starsForPoints.Add(star6);

              List<int> star7 = new List<int>();
              star7.Add(2);
              star7.Add(3);
              star7.Add(6);
              star7.Add(8);
              starsForPoints.Add(star7);

              List<int> star8 = new List<int>();
              star8.Add(1);
              star8.Add(2);
              star8.Add(7);
              star8.Add(9);
              starsForPoints.Add(star8);

              List<int> star9 = new List<int>();
              star9.Add(10);
              star9.Add(1);
              star9.Add(8);
              star9.Add(12);
              starsForPoints.Add(star9);

              List<int> star10 = new List<int>();
              star10.Add(11);
              star10.Add(9);
              star10.Add(12);
              starsForPoints.Add(star10);

              List<int> star11 = new List<int>();
              star11.Add(10);
              star11.Add(12);
              starsForPoints.Add(star11);

              List<int> star12 = new List<int>();
              star12.Add(11);
              star12.Add(10);
              star12.Add(1);
              star12.Add(9);
              starsForPoints.Add(star12);
          }
          */

        //получаем сисок точек из треугольноков
        private static List<Point> GetPointsFromTrianglesList(List<Triangle> triangles)
        {
            List<Point> points = new List<Point>();

            foreach (Triangle tr in triangles)
            {
                if (!points.Contains(tr.I)) { points.Add(tr.I); }
                if (!points.Contains(tr.J)) { points.Add(tr.J); }
                if (!points.Contains(tr.K)) { points.Add(tr.K); }
            }

            var ordered = points.OrderBy(x => x.Index);
            List<Point> orderedPoints = new List<Point>();
            foreach (Point pt in ordered)
            {
                orderedPoints.Add(pt);
            }
            return orderedPoints;

        }

        //Для каждой точки состявляем звезду из индексов точек
        private static List<List<int>> GetAllStars(List<Triangle> triangles, List<Point> orderedPoints)
        {
            List<List<int>> starsForPoints = new List<List<int>>();
            for (int i = 0; i < orderedPoints.Count; i++)
            {
                List<int> curStar = new List<int>();
                Point currentPoint = orderedPoints.ElementAt(i);
                foreach (Triangle tr in triangles)
                {
                    if (tr.I == currentPoint)
                    {
                        if (!curStar.Contains(tr.J.Index)) curStar.Add(tr.J.Index);
                        if (!curStar.Contains(tr.K.Index)) curStar.Add(tr.K.Index);
                        continue;
                    }
                    if (tr.J == currentPoint)
                    {
                        if (!curStar.Contains(tr.I.Index)) curStar.Add(tr.I.Index);
                        if (!curStar.Contains(tr.K.Index)) curStar.Add(tr.K.Index);
                        continue;
                    }
                    if (tr.K == currentPoint)
                    {
                        if (!curStar.Contains(tr.J.Index)) curStar.Add(tr.J.Index);
                        if (!curStar.Contains(tr.I.Index)) curStar.Add(tr.I.Index);
                        continue;
                    }
                }
                starsForPoints.Add(curStar);
            }
            return starsForPoints;
        }

        //Получаем список индексов точек, из которых будем выбирать первый узел
        private static void GetFirstNodePretenders(int prevIndex, int prevNodesCount, List<IndexWithNodesCount> nodes, List<List<int>> starsForPoints, List<Point> orderedPoints)
        {
            List<int> allIndexes = new List<int>();
            allIndexes.Add(prevIndex + 1);
            List<int> indexesOnTeCurrentLevel = new List<int>();
            List<int> indexesOnTeNextLevel = new List<int>();
            foreach (int ind in starsForPoints[prevIndex]) { indexesOnTeCurrentLevel.Add(ind); }
            foreach (int ind in indexesOnTeCurrentLevel) { allIndexes.Add(ind); }
            int count = orderedPoints.Count;
            int nodesCount = 1;
            while (allIndexes.Count < count)
            {
                foreach (int ind in indexesOnTeCurrentLevel)
                {
                    foreach (int neiborNode in starsForPoints[ind - 1])
                    {
                        if (!allIndexes.Contains(neiborNode))
                        {
                            allIndexes.Add(neiborNode);
                            indexesOnTeNextLevel.Add(neiborNode);
                        }
                    }
                }
                nodesCount++;
                indexesOnTeCurrentLevel.Clear();
                foreach (int elem in indexesOnTeNextLevel)
                { indexesOnTeCurrentLevel.Add(elem); }
                indexesOnTeNextLevel.Clear();
            }

            IndexWithNodesCount element = new IndexWithNodesCount(prevIndex, nodesCount);
            nodes.Add(element);

            if (nodesCount > prevNodesCount)
            {
                int minNeibors = starsForPoints[indexesOnTeCurrentLevel.ElementAt(0) - 1].Count;
                foreach (int ind in indexesOnTeCurrentLevel)
                {
                    if (starsForPoints[ind - 1].Count < minNeibors) minNeibors = starsForPoints[ind - 1].Count;
                }

                foreach (int ind in indexesOnTeCurrentLevel)
                {
                    if (starsForPoints[ind - 1].Count == minNeibors) GetFirstNodePretenders(ind - 1, nodesCount, nodes, starsForPoints, orderedPoints);
                }
            }
        }

        //Выбираем индекс первого узла
        private static int GetFirstPointIndex(List<IndexWithNodesCount> nodes)
        {
            //GetFirstNodePretenders(0,0);
            int maxNodes = 0;
            int index = 0;
            foreach (IndexWithNodesCount curNode in nodes)
            {
                if (curNode.NodeCount > maxNodes)
                {
                    maxNodes = curNode.NodeCount;
                    index = curNode.Index;
                }
            }
            return index;
        }

        //Упорядочиваем индексы точек в звезде по неубыванию связей
        private static void GetAscendingOrderedStars(List<List<int>> starsForPoints)
        {
            foreach (List<int> curStar in starsForPoints)
            {
                for (int i = 0; i < curStar.Count; i++)
                {
                    for (int j = curStar.Count - 1; j > i; j--)
                    {
                        if (starsForPoints[curStar[j - 1] - 1].Count > starsForPoints[curStar[j] - 1].Count)
                        {
                            int buf = curStar[j - 1];
                            curStar[j - 1] = curStar[j];
                            curStar[j] = buf;
                        }
                    }
                }
            }
        }

        //Собственно, перенумерация
        public static void DoRenumeration(List<Triangle> allTriangles)
        {
            List<Triangle> triangles = allTriangles;

            List<Point> orderedPoints = GetPointsFromTrianglesList(triangles);
            List<List<int>> starsForPoints = GetAllStars(triangles, orderedPoints);
            GetAscendingOrderedStars(starsForPoints);

            List<IndexWithNodesCount> nodes = new List<IndexWithNodesCount>();
            GetFirstNodePretenders(0, 0, nodes, starsForPoints, orderedPoints);
            int currentIndex = GetFirstPointIndex(nodes);
            int pointsCount = orderedPoints.Count;
            List<int> orderedIndexes = new List<int>();
            orderedIndexes.Add(currentIndex + 1);
            int reorderedCount = 1;

            while (reorderedCount != pointsCount)
            {
                List<int> currentStar = starsForPoints[currentIndex];
                for (int i = 0; i < currentStar.Count; i++)
                {
                    if (!orderedIndexes.Contains(currentStar[i])) orderedIndexes.Add(currentStar[i]);
                }
                currentIndex = orderedIndexes[reorderedCount] - 1;
                reorderedCount++;
            }

            List<Point> reorderedPoints = new List<Point>();

            for (int i = pointsCount - 1; i >= 0; i--) reorderedPoints.Add(orderedPoints[orderedIndexes[i] - 1].Clone());

            for (int i = 0; i < orderedPoints.Count; i++)
            {
                orderedPoints[reorderedPoints[i].Index - 1].Index = i + 1;
            }
        }
       
    }
}
