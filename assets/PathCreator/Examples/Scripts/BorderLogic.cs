using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PathCreation.Examples
{
    public class BorderLogic
    {


        void Generate()
        {

        }



        //   BigBorder (lenght, smallBorderCount)
        //   Kneht(l)
        //   Space(l)

        //  ObjectsSequence = List < Objects >
        //    AllObjects = List < SegmentObjects >

        //for (segment)
        //   {
        //      segmentLenght

        //      segmentObjects = new SegmentObjects();

        /*   // Добавлем по элементу и проверяем остаток длины
           while (true)
           {
               if (BigBorder.leght < segmentLenght - GetLeghtOfObjects())
                   segmentObjects.Add(new BigBorder)

           if (Space.lenght < )
       }

           // Добавляем маленькие бордюры

           // Расчитываем spacing

           // Добавляем объекты в общий лист
           AllObjects.Add(segmentObjects)
       }



       float GetLeghtOfObjects(objectSequence)
       {
           return sumLenght;
       }

       SettingObjects(List<Segment>, List<SecgmentObjects>)
       */
    }

    public class Segment
    {
        Vector3 startPoint;
        Vector3 endPoint;

        public Segment(Vector3 start, Vector3 end)
        {
            startPoint = start;
            endPoint = end;
        }

    }

    public class Border
    {

    }
}
