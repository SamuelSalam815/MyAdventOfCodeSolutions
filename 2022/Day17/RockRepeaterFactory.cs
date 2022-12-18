namespace Day17;

public static class RockRepeaterFactory
{
        public static Repeater<Rock> GetRepeater()
        {
                List<Rock> rockSequence = new()
                {
                        new() // Dash '-'
                        {
                                new(0,0),
                                new(1,0),
                                new(2,0),
                                new(3,0),
                        },
                        new() // Plus '+'
                        {
                                new(1,2),
                                new(0,1),
                                new(1,1),
                                new(2,1),
                                new(1,0),
                        },
                        new() // Backwards L
                        {
                                new(2,2),
                                new(2,1),
                                new(2,0),
                                new(1,0),
                                new(0,0),
                        },
                        new() // Vertical bar '|'
                        {
                                new(0,3),
                                new(0,2),
                                new(0,1),
                                new(0,0),
                        },
                        new() // 2x2 box
                        {
                                new(0,1),
                                new(1,1),
                                new(0,0),
                                new(1,0),
                        },
                };

                return new Repeater<Rock>(rockSequence);
        }
}
