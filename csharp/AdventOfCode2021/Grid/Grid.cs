using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Grid
{
    public class Grid<TCell>
    {
        private readonly Dictionary<(int x, int y), TCell> _data = new Dictionary<(int x, int y), TCell>();
        
        public string ToString(Func<TCell, char> renderFunc)
        {
            var maxX = _data.Keys.Select(p => p.x).Max();
            var maxY = _data.Keys.Select(p => p.y).Max();

            return string.Join("\n", Enumerable.Range(0, maxY + 1).Select(y =>
            {
                return Enumerable.Range(0, maxX + 1).Select(x => renderFunc(_data.GetValueOrDefault((x, y))));
            }).Select(chars => new string(chars.ToArray())));
        }
    }
}
