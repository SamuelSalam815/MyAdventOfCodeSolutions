Goal: Maximize production of geodes

Current approach: Brute force + culling of the decision tree

- Brute Force : make every possible decision
    - A decision is either to idle until the time limit is reached, or to wait for enough resources to build a particular kind of robot


- Decision Culling Procedure:
    - Do not build a robot if the resource it gathers will be wasted.

    - Resource production is wasted if the resource will never be consumed OR is not a geode.

    - Any resource production above the maximum rate of consumption is wasted.

    - The maximum rate of consumption is the maximum resource requirement for that type of resource across the entire blueprint each minute
        
        - Justification: 
        - Can only build one robot at a time
        - Building any robot takes 1 minute. 
        - Therefore maximum resource consumption for this type of resource is achieved by building the robot that requires the most of this resource every minute.

Problem: Current approach is too slow to solve part2 of the problem in a reasonable amount of time

- Idea: come up with a non-brute force algorithm
    - Not promising
- Idea: improve the decision culling procedure
    - Use a priority queue to make the decisions that look the best first, then use a heuristic which will upper bound the max geodes produced from a given state to ignore bad branches of the decision tree after backtracking
    - Problem: how to get a tight upper bound. If the bound is not tight enough, this approach is not helpful.

---

Example in Part 1 : An optimal approach leads to the following decisions
- Build 3 clay robots
- Build 1 obsidian robot
- Build 1 clay robot
- Build 1 obsidian robot
- Build 2 geode robots
- Idle until time limit

First assumption: 
- You can get an optimal solution without interleaving the construction of different robot types
    - for example you can get an optimal solution by first building all the ore robots needed, then all the clay robots, then all the obsidian robots, and finally all the geode robots
- The example in part 1 shows this assumption to be wrong
    - why ?
    - minimizing time to build geode robots should maximize the number of geodes produced
    - inspecting the time to build for each robot type at each time step

Minutes | ORE | CLY | OBS | GEO |
---
  1     | 5   | 3   | inf | inf |
  2     | 4   | 2   | inf | inf |
  3     | 3   | 1   | inf | inf |
--- Build Clay robot
  4     | 4   | 2   | 14  | inf |
  5     | 3   | 1   | 13  | inf |
--- Build Clay robot
  6     | 4   | 2   | 7   | inf |
  7     | 3   | 1   | 6   | inf |
--- Build Clay robot
  8     | 4   | 2   | 4   | inf |
  9     | 3   | 1   | 3   | inf |
  10    | 2   | 1   | 2   | inf |
  11    | 1   | 1   | 1   | inf |
--- Build Obsidian robot