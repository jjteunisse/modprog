using System;
using System.Collections.Generic;

class Node {
    public string p, s;
    public Node(string _p, string _s) { p = _p;  s = _s; }
    public bool Sorted() {
        for (int i = 0; i < s.Length - 1; i++) if (s[i] > s[i + 1]) return false;
        return true;
    }
}

class Tricky {
    Queue<Node> next;
    HashSet<string> uniques;

    Node GetNext(Node N) {
        string s = "" + N.s[1] + N.s[0] + N.s.Substring(2);
        if (!uniques.Contains(s)) { uniques.Add(s); next.Enqueue(new Node(N.p + "a", s)); }

        s = N.s.Substring(0, N.s.Length - 2) + N.s[N.s.Length - 1] + N.s[N.s.Length - 2];
        if (!uniques.Contains(s)) { uniques.Add(s); next.Enqueue(new Node(N.p + "b", s)); }

        s = "" + N.s[0] + N.s[N.s.Length - 2];
        for (int i = 1; i < N.s.Length - 2; i++) s += N.s[i];
        s += N.s[N.s.Length - 1];
        if (!uniques.Contains(s)) { uniques.Add(s); next.Enqueue(new Node(N.p + "x", s)); }

        return next.Dequeue();
    }
    
    static void Main(){
        Tricky T = new Tricky();
        for (int i = Convert.ToInt32(Console.ReadLine()); i > 0; i--) {
            T.next = new Queue<Node>();
            T.uniques = new HashSet<string>();
            Node N = new Node("", Console.ReadLine());
            while (!N.Sorted()) N = T.GetNext(N);
            Console.WriteLine(N.p.Length + " " + N.p);
        }
    }
}
