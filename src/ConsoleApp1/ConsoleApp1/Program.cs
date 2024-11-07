using System;

namespace RoguelikeGame
{
    
    public class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Aid Aid { get; set; }
        public Weapon Weapon { get; set; }
        public int Score { get; set; }

        public Player(string name, int maxHealth)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = MaxHealth;
            Score = 0;
        }

        public void Heal(Aid aid) => Health = Math.Min(Health + aid.HealAmount, MaxHealth);

        public void Attack(Enemy enemy) => enemy.Health -= Weapon.Damage;
    }

    
    public class Enemy
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Weapon Weapon { get; set; }

        public Enemy(string name, int maxHealth, Weapon weapon)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = MaxHealth;
            Weapon = weapon;
        }

        public void Attack(Player player) => player.Health -= Weapon.Damage;
    }

    
    public class Aid
    {
        public string Name { get; set; }
        public int HealAmount { get; set; }

        public Aid(string name, int healAmount)
        {
            Name = name;
            HealAmount = healAmount;
        }
    }

    
    public class Weapon
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Durability { get; set; }

        public Weapon(string name, int damage, int durability)
        {
            Name = name;
            Damage = damage;
            Durability = durability;
        }
    }

    class Program
    {
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать, воин!");
            Console.Write("Назови себя: ");
            string playerName = Console.ReadLine();

            
            Player player = new Player(playerName, 100);

            
            player.Weapon = GenerateWeapon();
            player.Aid = GenerateAid();

            Console.WriteLine($"Ваше имя {playerName}!");
            Console.WriteLine($"Вам был ниспослан меч {player.Weapon.Name} ({player.Weapon.Damage}), а также {player.Aid.Name} ({player.Aid.HealAmount}hp).");
            Console.WriteLine($"У вас {player.Health}hp.");

            
            while (player.Health > 0)
            {
            
                Enemy enemy = GenerateEnemy();

                Console.WriteLine($"{player.Name} встречает врага {enemy.Name} ({enemy.Health}hp), у врага на поясе сияет оружие {enemy.Weapon.Name} ({enemy.Weapon.Damage})");

                
                while (player.Health > 0 && enemy.Health > 0)
                {
                    Console.WriteLine("Что вы будете делать?");
                    Console.WriteLine("1. Ударить");
                    Console.WriteLine("2. Пропустить ход");
                    Console.WriteLine("3. Использовать аптечку");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            player.Attack(enemy);
                            Console.WriteLine($"{player.Name} ударил противника {enemy.Name}");
                            if (enemy.Health > 0)
                            {
                                enemy.Attack(player);
                                Console.WriteLine($"Противник {enemy.Name} ударил вас!");
                            }
                            break;
                        case "2":
                            Console.WriteLine($"{player.Name} пропустил ход.");
                            enemy.Attack(player);
                            Console.WriteLine($"Противник {enemy.Name} ударил вас!");
                            break;
                        case "3":
                            player.Heal(player.Aid);
                            Console.WriteLine($"{player.Name} использовал аптечку");
                            break;
                        default:
                            Console.WriteLine("Неверный выбор!");
                            break;
                    }

                    Console.WriteLine($"У противника {enemy.Health}hp, у вас {player.Health}hp");
                }

                if (enemy.Health <= 0)
                {
                    player.Score += enemy.MaxHealth;
                    Console.WriteLine($"Вы победили {enemy.Name}! Вы получили {enemy.MaxHealth} очков!");
                }

                if (player.Health <= 0)
                {
                    Console.WriteLine("К сожалению, вы проиграли.");
                    break;
                }
            }

            Console.WriteLine($"Игра окончена. Ваш счёт: {player.Score}");
            Console.ReadKey();
        }

        
        static Weapon GenerateWeapon()
        {
            string[] names = { "Фламберг", "Экскалибур", "Меч Звезды", "Кинжал", "Топор" };
            int damage = rnd.Next(10, 30);
            int durability = rnd.Next(1, 10);

            return new Weapon(names[rnd.Next(names.Length)], damage, durability);
        }

        
        static Aid GenerateAid()
        {
            string[] names = { "Средняя аптечка", "Большая аптечка", "Малая аптечка" };
            int healAmount = rnd.Next(5, 20);

            return new Aid(names[rnd.Next(names.Length)], healAmount);
        }

        
        static Enemy GenerateEnemy()
        {
            string[] names = { "Варвар", "Орк", "Голем", "Скелет", "Зомби" };
            int maxHealth = rnd.Next(20, 80);
            Weapon weapon = GenerateWeapon();

            return new Enemy(names[rnd.Next(names.Length)], maxHealth, weapon);
        }
    }
}
