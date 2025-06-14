using SportifyApi.Models;

namespace SportifyApi.Data
{
    public static class DbSeeder
    {
        public static void SeedAchievements(AppDbContext context)
        {
            var predefined = new List<Achievement>
            {
                // ────────────────
                // 🎯 MANUAL SPORT-SPECIFIC (Assigned only by admin userId == 2)
                // ────────────────
                new() { AchievementId = 1, Title = "🏉 Man of the Match", Description = "Top-performing player", SportType = "Rugby", IconUrl = "https://cdn-icons-png.flaticon.com/512/1055/1055672.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 2, Title = "💪 Best Tackler", Description = "Most effective in defense", SportType = "Rugby", IconUrl = "https://cdn-icons-png.flaticon.com/512/2848/2848919.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 3, Title = "⚽ Top Scorer", Description = "Most goals scored", SportType = "Soccer", IconUrl = "https://cdn-icons-png.flaticon.com/512/883/883407.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 4, Title = "🧤 Best Goalkeeper", Description = "Outstanding defensive performance", SportType = "Soccer", IconUrl = "https://cdn-icons-png.flaticon.com/512/2886/2886696.png", Points = 100,IsAutoGenerated = false },
                new() { AchievementId = 5, Title = "🏆 MVP", Description = "Most valuable player", SportType = "Soccer", IconUrl = "https://cdn-icons-png.flaticon.com/512/1055/1055646.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 6, Title = "🥇 First Place", Description = "Winner of the race", SportType = "Running", IconUrl = "https://cdn-icons-png.flaticon.com/512/3514/3514491.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 7, Title = "🥈 Second Place", Description = "Runner-up", SportType = "Running", IconUrl = "https://cdn-icons-png.flaticon.com/512/2583/2583427.png", Points = 80, IsAutoGenerated = false },
                new() { AchievementId = 8, Title = "🥉 Third Place", Description = "Third-place finisher", SportType = "Running", IconUrl = "https://cdn-icons-png.flaticon.com/512/2583/2583428.png", Points = 50, IsAutoGenerated = false },
                new() { AchievementId = 9, Title = "🏊 Best Swimmer", Description = "Fastest swimmer", SportType = "Swimming", IconUrl = "https://cdn-icons-png.flaticon.com/512/3208/3208744.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 10, Title = "🏀 MVP", Description = "Top player of the match", SportType = "Basketball", IconUrl = "https://cdn-icons-png.flaticon.com/512/2891/2891137.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 11, Title = "🎯 Most 3-Pointers", Description = "Most successful 3-point shots", SportType = "Basketball", IconUrl = "https://cdn-icons-png.flaticon.com/512/3138/3138533.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 12, Title = "🏏 Best Batsman", Description = "Top run scorer", SportType = "Cricket", IconUrl = "https://cdn-icons-png.flaticon.com/512/2937/2937996.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 13, Title = "🔥 Best Bowler", Description = "Most wickets taken", SportType = "Cricket", IconUrl = "https://cdn-icons-png.flaticon.com/512/2937/2937933.png", Points = 80, IsAutoGenerated = false },
                new() { AchievementId = 14, Title = "🎾 Ace Leader", Description = "Most aces served", SportType = "Tennis", IconUrl = "https://cdn-icons-png.flaticon.com/512/1736/1736343.png", Points = 80, IsAutoGenerated = false },
                new() { AchievementId = 15, Title = "🏓 Champion", Description = "Tournament winner", SportType = "Table Tennis", IconUrl = "https://cdn-icons-png.flaticon.com/512/3964/3964806.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 16, Title = "🥋 Best Technique", Description = "Cleanest moves", SportType = "Judo", IconUrl = "https://cdn-icons-png.flaticon.com/512/2172/2172383.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 17, Title = "🚴 King of the Mountain", Description = "Best climber", SportType = "Cycling", IconUrl = "https://cdn-icons-png.flaticon.com/512/235/235861.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 18, Title = "🏇 Fastest Finish", Description = "Top performer", SportType = "Horse Racing", IconUrl = "https://cdn-icons-png.flaticon.com/512/1287/1287710.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 19, Title = "🥊 Knockout King", Description = "Best boxer", SportType = "Boxing", IconUrl = "https://cdn-icons-png.flaticon.com/512/1394/1394386.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 20, Title = "🔥 Fastest Try", Description = "Scored a try in record time", SportType = "Rugby", IconUrl = "https://cdn-icons-png.flaticon.com/512/1046/1046890.png", Points = 150, IsAutoGenerated = false },
                new() { AchievementId = 21, Title = "🛡️ Iron Wall", Description = "Unbreakable defense throughout the match", SportType = "Rugby", IconUrl = "https://cdn-icons-png.flaticon.com/512/1617/1617633.png", Points = 120, IsAutoGenerated = false },
                new() { AchievementId = 22, Title = "🎯 Assist King", Description = "Most assists in the match", SportType = "Soccer", IconUrl = "https://cdn-icons-png.flaticon.com/512/1036/1036294.png", Points = 90, IsAutoGenerated = false },
                new() { AchievementId = 23, Title = "🚀 Long Shot Goal", Description = "Scored from outside the box", SportType = "Soccer", IconUrl = "https://cdn-icons-png.flaticon.com/512/2934/2934087.png", Points = 50, IsAutoGenerated = false },
                new() { AchievementId = 24, Title = "🛡️ Defensive Beast", Description = "Most rebounds and blocks", SportType = "Basketball", IconUrl = "https://cdn-icons-png.flaticon.com/512/1170/1170627.png", Points = 50, IsAutoGenerated = false },
                new() { AchievementId = 25, Title = "💨 Fast Break Leader", Description = "Led multiple fast breaks", SportType = "Basketball", IconUrl = "https://cdn-icons-png.flaticon.com/512/1055/1055671.png", Points = 80, IsAutoGenerated = false },
                new() { AchievementId = 26, Title = "🌀 Smoothest Stroke", Description = "Best stroke form", SportType = "Swimming", IconUrl = "https://cdn-icons-png.flaticon.com/512/2309/2309706.png", Points = 40, IsAutoGenerated = false },
                new() { AchievementId = 27, Title = "💪 Stamina Master", Description = "Completed a long-distance swim", SportType = "Swimming", IconUrl = "https://cdn-icons-png.flaticon.com/512/2784/2784415.png", Points = 50, IsAutoGenerated = false },
                new() { AchievementId = 28, Title = "🧤 Safe Hands", Description = "Most catches taken", SportType = "Cricket", IconUrl = "https://cdn-icons-png.flaticon.com/512/924/924914.png", Points = 70, IsAutoGenerated = false },
                new() { AchievementId = 29, Title = "🚀 Six Machine", Description = "Most sixes hit", SportType = "Cricket", IconUrl = "https://cdn-icons-png.flaticon.com/512/1046/1046885.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 30, Title = "⚡ Speed Serve", Description = "Fastest serve", SportType = "Table Tennis", IconUrl = "https://cdn-icons-png.flaticon.com/512/3135/3135710.png", Points = 50, IsAutoGenerated = false },
                new() { AchievementId = 31, Title = "🎯 Spin Master", Description = "Best spin shots", SportType = "Table Tennis", IconUrl = "https://cdn-icons-png.flaticon.com/512/3474/3474270.png", Points = 20, IsAutoGenerated = false },
                new() { AchievementId = 32, Title = "🥎 Return King", Description = "Most successful returns", SportType = "Tennis", IconUrl = "https://cdn-icons-png.flaticon.com/512/1712/1712490.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 33, Title = "🔨 Smash Shot", Description = "Powerful overhead smashes", SportType = "Tennis", IconUrl = "https://cdn-icons-png.flaticon.com/512/1864/1864519.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 34, Title = "🥋 Kata Master", Description = "Exceptional form in kata", SportType = "Karate", IconUrl = "https://cdn-icons-png.flaticon.com/512/2331/2331570.png", Points = 50, IsAutoGenerated = false },
                new() { AchievementId = 35, Title = "🥊 Lightning Reflexes", Description = "Quickest counter attacks", SportType = "Karate", IconUrl = "https://cdn-icons-png.flaticon.com/512/921/921917.png", Points = 20, IsAutoGenerated = false },
                new() { AchievementId = 36, Title = "🏹 Bullseye!", Description = "Hit the bullseye", SportType = "Archery", IconUrl = "https://cdn-icons-png.flaticon.com/512/3062/3062634.png", Points = 50, IsAutoGenerated = false },
                new() { AchievementId = 37, Title = "🎯 Most Accurate", Description = "Highest accuracy score", SportType = "Archery", IconUrl = "https://cdn-icons-png.flaticon.com/512/3062/3062633.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 38, Title = "🧘 Balance Guru", Description = "Held the toughest poses", SportType = "Yoga", IconUrl = "https://cdn-icons-png.flaticon.com/512/4152/4152381.png", Points = 40, IsAutoGenerated = false },
                new() { AchievementId = 39, Title = "🌬️ Breath Master", Description = "Controlled breath for entire session", SportType = "Yoga", IconUrl = "https://cdn-icons-png.flaticon.com/512/4167/4167658.png", Points = 20, IsAutoGenerated = false },
                new() { AchievementId = 40, Title = "🛹 Trick King", Description = "Landed multiple advanced tricks", SportType = "Skateboarding", IconUrl = "https://cdn-icons-png.flaticon.com/512/5091/5091098.png", Points = 50, IsAutoGenerated = false },
                new() { AchievementId = 41, Title = "🔥 Rail Shredder", Description = "Dominated the rails", SportType = "Skateboarding", IconUrl = "https://cdn-icons-png.flaticon.com/512/5091/5091087.png", Points = 20, IsAutoGenerated = false },
                new() { AchievementId = 42, Title = "🏒 Power Shooter", Description = "Fastest slapshot", SportType = "Hockey", IconUrl = "https://cdn-icons-png.flaticon.com/512/5207/5207804.png", Points = 40, IsAutoGenerated = false },
                new() { AchievementId = 43, Title = "🧤 Save Master", Description = "Best goalie performance", SportType = "Hockey", IconUrl = "https://cdn-icons-png.flaticon.com/512/3059/3059894.png", Points = 70, IsAutoGenerated = false },
                new() { AchievementId = 44, Title = "👐 Best Setter", Description = "Flawless setups all game", SportType = "Volleyball", IconUrl = "https://cdn-icons-png.flaticon.com/512/3022/3022474.png", Points = 100, IsAutoGenerated = false },
                new() { AchievementId = 45, Title = "💥 Spike Master", Description = "Most spikes landed", SportType = "Volleyball", IconUrl = "https://cdn-icons-png.flaticon.com/512/1133/1133734.png", Points = 20, IsAutoGenerated = false },
                new() { AchievementId = 46, Title = "🏌️ Hole-in-One", Description = "Scored a hole-in-one", SportType = "Golf", IconUrl = "https://cdn-icons-png.flaticon.com/512/1047/1047799.png", Points = 200, IsAutoGenerated = false },
                new() { AchievementId = 47, Title = "🧠 Course Strategist", Description = "Smartest game play", SportType = "Golf", IconUrl = "https://cdn-icons-png.flaticon.com/512/1022/1022740.png", Points = 50, IsAutoGenerated = false },



                // ────────────────
                // 🤖 AUTO ACHIEVEMENTS (triggered by participation/time-based behavior)
                // ────────────────
                new() { AchievementId = 48, Title = "First Event Joined", Description = "Joined your first event!", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/1041/1041916.png", Points = 50, IsAutoGenerated = true },
                new() { AchievementId = 49, Title = "Joined 2 Events", Description = "You're getting warmed up!", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/565/565547.png", Points = 60, IsAutoGenerated = true },
                new() { AchievementId = 50, Title = "Joined 3 Events", Description = "Nice streak!", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/565/565545.png", Points = 70, IsAutoGenerated = true },
                new() { AchievementId = 51, Title = "Joined 5 Events", Description = "You're starting to get active!", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/4380/4380890.png", Points = 80, IsAutoGenerated = true },
                new() { AchievementId = 52, Title = "10 Events Joined", Description = "Double digits!", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/595/595890.png", Points = 90, IsAutoGenerated = true },
                new() { AchievementId = 53, Title = "100 Events Joined", Description = "You've joined 100 events!", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/1422/1422519.png", Points = 100, IsAutoGenerated = true },
                new() { AchievementId = 54, Title = "Joined 15 Events", Description = "You're officially committed!", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/1087/1087815.png", Points = 65, IsAutoGenerated = true },
                new() { AchievementId = 55, Title = "Joined 25 Events", Description = "A quarter to 100!", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/4442/4442732.png", Points = 75, IsAutoGenerated = true },
                new() { AchievementId = 56, Title = "Joined 50 Events", Description = "Half a century!", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/2022/2022665.png", Points = 85, IsAutoGenerated = true },

                // Time-based
                new() { AchievementId = 57, Title = "Weekend Warrior", Description = "Joined an event on a Saturday or Sunday", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/1055/1055646.png", Points = 20, IsAutoGenerated = true },
                new() { AchievementId = 58, Title = "Night Owl", Description = "Joined an event that starts after 8 PM", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/1164/1164690.png", Points = 20, IsAutoGenerated = true },
                new() { AchievementId = 59, Title = "Early Bird", Description = "Joined an event before 7 AM", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/2947/2947995.png", Points = 20, IsAutoGenerated = true },
                new() { AchievementId = 60, Title = "First Event Hosted", Description = "You created your first event!", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/992/992651.png", Points = 20, IsAutoGenerated = true },
                new() { AchievementId = 61, Title = "First Approval Received", Description = "Someone approved your event participation", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/1828/1828506.png", Points = 20, IsAutoGenerated = true },
                new() { AchievementId = 62, Title = "First Achievement Earned", Description = "You've unlocked your first achievement!", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/1828/1828919.png", Points = 20, IsAutoGenerated = true },
                new() { AchievementId = 63, Title = "Invited a Friend", Description = "You invited another user to an event", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/1828/1828490.png", Points = 20, IsAutoGenerated = true },
                new() { AchievementId = 64, Title = "Joined an Event with 5+ Users", Description = "You're part of a big event!", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/744/744984.png", Points = 20, IsAutoGenerated = true },
                new() { AchievementId = 65, Title = "New Year Starter", Description = "Joined an event in January", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/979/979585.png", Points = 20, IsAutoGenerated = true },
                new() { AchievementId = 66, Title = "Holiday Hustler", Description = "Joined an event on a public holiday", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/1055/1055651.png", Points = 20, IsAutoGenerated = true },
                new() { AchievementId = 67, Title = "3 Events in 7 Days", Description = "You've joined 3 events in a week!", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/1584/1584892.png", Points = 20, IsAutoGenerated = true },
                new() { AchievementId = 68, Title = "Back-to-Back Events", Description = "Joined events on two consecutive days", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/476/476863.png", Points = 20, IsAutoGenerated = true },
                new() { AchievementId = 69, Title = "Late Bloomer", Description = "Joined your first event after 30 days on Sportify", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/3757/3757296.png", Points = 20, IsAutoGenerated = true },
                new() { AchievementId = 70, Title = "Achievement Hunter", Description = "You've earned 5 achievements", SportType = "General", IconUrl = "https://cdn-icons-png.flaticon.com/512/2311/2311524.png", Points = 20, IsAutoGenerated = true },


            };

// 🧹 1. Clear existing achievements
context.Achievements.RemoveRange(context.Achievements);
context.SaveChanges();

// ✅ 2. Add predefined achievements
foreach (var achievement in predefined)
{
    context.Achievements.Add(achievement);
}

// 💾 3. Save changes
context.SaveChanges();
        }
    }
}   
