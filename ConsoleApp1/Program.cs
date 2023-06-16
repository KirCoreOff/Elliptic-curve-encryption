using static System.Console;

class Program
{
    static int inv(int a, int m)
    {
        if (a == 1)
            return 1;
        if (a == 0)
            return -1;
        return (1 - 1 * inv(m % a, a) * m) / a + m;
    }

    static (int, int) FindPoints(int x, int y, int k, int a, int p) //нахождение коортинат точки на эллиптической кривой
    {
        int oldx = x;
        int oldy = y;
        int l;
        int reverse;
        //WriteLine($"{x} {y}");
        for (int i = 1; i < k; i++)
        {
            if (x == -1)
            {
                x = oldx;
                y = oldy;
            }
            else
            {
                if ((x == oldx) && (y == oldy))
                {
                    reverse = inv(2 * oldy, p);
                    l = (3 * oldx * oldx + a) * reverse % p;
                }
                else
                {
                    reverse = inv((x - oldx) < 0 ? p + (x - oldx) : x - oldx, p);
                    l = (y - oldy) * reverse % p;
                }
                if (reverse == -1)
                {
                    x = -1;
                    y = -1;
                }
                else
                {
                    if (l < 0)
                        l = p + l;
                    x = (l * l - x - oldx) % p;
                    if (x < 0)
                        x = p + x;
                    y = (l * (oldx - x) - oldy) % p;
                    if (y < 0)
                        y = p + y;
                }
            }
            //WriteLine($"{x} {y}");
        }
        return (x, y);
    }

    static (int, int) SumPoints(int x1, int y1, int x2, int y2, int a, int p) //сложение точек на эллиптической кривой
    {
        int x, y;
        int reverse;
        int l;
        if ((x1 == -1) || (x2 == -1))
            if(x1 == -1)      
                return (x2, y2);
            else return (x1, y1);
            
        if ((x1 == x2) && (y1 == y2))
        {
            reverse = inv(2 * y1, p);
            l = (3 * x1 * x1 + a) * reverse % p;
        }
        else
        {
            reverse = inv((x2-x1)<0? p+(x2-x1) : x2 - x1, p);
            l = (y2 - y1) * reverse % p;
        }
        if (reverse == -1)
        {
            x = -1;
            y = -1;
        }
        else
        {
            if (l < 0)
                l = p + l;
            x = (l * l - x2 - x1) % p;
            if (x < 0)
                x = p + x;
            y = (l * (x1 - x) - y1) % p;
            if (y < 0)
                y = p + y;
        }
        return (x, y);
    }

    static void Main(string[] args)
    {
        Dictionary<(int, int), char> alpha = new Dictionary<(int, int), char>() { { (1, 13), 'a' }, { (1, 28), 'b' }, { (7, 5), 'c' },
            { (7, 36) ,'d' },  {(12, 12), 'e' }, { (12, 29), 'f'}, {(15, 20), 'g'}, {(15, 21), 'h'}, {(23, 4), 'i'}, {(23, 37), 'j'}, {(27, 19), 'k'}, { (27, 22), 'l'}, {(-1, -1), ' '}};

        string msg = "hi dal dal gad"; //сообщение состоит из символов созданного алфавита
        int p = 41;
        int a = 1;
        int b = 3;
        int n = 13;
        int gx = 7;
        int gy = 5;
        int k = 10;
        WriteLine($"Message: {msg}");
        //int px = 1; //
        //int py = 5; //отркытай ключ собеседника
        var (px, py) = FindPoints(gx, gy, 5, a, p); // открытый ключ собеседника

        string encrypted_msg = "";
        var (my_open_x, my_open_y) = FindPoints(gx, gy, k, a, p);
        WriteLine($"My public key point: x = {my_open_x} y = {my_open_y}");
        var (secret_x, secrety_y) = FindPoints(px, py, k, a, p);
        foreach (char c in msg) //шифрование сообщения
        {
            var (mx, my) = alpha.Where(x => x.Value == c).FirstOrDefault().Key;
            var (encrypt_x, encrypt_y) = SumPoints(secret_x, secrety_y, mx, my, a, p);
            encrypted_msg += (alpha[(encrypt_x, encrypt_y)]);
        }
        WriteLine($"Encrypted message: {encrypted_msg}");

        string decrypted_msg = "";
        foreach(char c in encrypted_msg) //расшифрование сообщения
        {
            var (encrypt_x, encrypt_y) = alpha.Where(x => x.Value == c).FirstOrDefault().Key;
            var (decrypt_x, decrypt_y) = SumPoints(secret_x, -secrety_y , encrypt_x, encrypt_y, a, p);
            decrypted_msg += (alpha[(decrypt_x, decrypt_y)]);
        }
        WriteLine($"Decrypted message: {decrypted_msg}");
    }
}