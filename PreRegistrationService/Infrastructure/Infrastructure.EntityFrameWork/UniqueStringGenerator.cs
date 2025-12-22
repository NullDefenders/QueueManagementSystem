using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityFrameWork
{
    public sealed class UniqueStringGenerator
    {
        private const long MAX = 3486784401;

        private readonly Random _random;
        private readonly HashSet<long> _history;

        public UniqueStringGenerator(int seed)
        {
            _random = new Random(seed);
            _history = new HashSet<long>();
        }

        public UniqueStringGenerator()
        {
            _random = new Random();
            _history = new HashSet<long>();
        }

        public string Next() => Format(NextNumber());

        public void Reset()
        {
            _history.TrimExcess();
            _history.Clear();

        }

        private long NextNumber()
        {

            if (_history.Count >= int.MaxValue)
            {
                throw new InvalidOperationException("Variants exceeded. Please reset");
            }
            var next = _random.NextInt64(0, MAX);
            while (_history.Contains(next))
            {
                next = _random.NextInt64(0, MAX);
            }
            _history.Add(next);
            return next;
        }

        private static string Format(long number) => string.Create<long>(5, number,
            static (span, number) =>
            {
                const string DICTIONARY = "123456789";
                for (var i = 0; i < span.Length; i++)
                {
                    var range = 0..DICTIONARY.Length;
                    while (!range.Start.Equals(range.End.Value))
                    {
                        var length = range.End.Value - range.Start.Value;
                        var step = length / 2;
                        if (step == 0)
                        {
                            break;
                        }
                        if ((number & 1) == 0)
                        {
                            range = range.Start..(range.End.Value - step);
                        }
                        else
                        {
                            range = (range.Start.Value + step)..range.End;
                        }
                        number >>= 1;
                    }
                    span[i] = DICTIONARY[range.Start];
                }
            });
    }
}
