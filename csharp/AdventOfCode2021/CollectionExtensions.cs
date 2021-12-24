﻿using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    public static class CollectionExtensions
    {
        public static IEnumerable<(TFirst, TSecond)> CrossProduct<TFirst, TSecond>(this ICollection<TFirst> first,
            ICollection<TSecond> second)
        {
            return
                from firstItem in first
                from secondItem in second
                select (firstItem, secondItem);
        }
    }
}