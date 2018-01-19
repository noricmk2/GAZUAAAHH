using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace _20180104
{
    class Client
    {
        private int id;
        public delegate void clientService(object sender, EventArgs args); //delegate 함수포인터랑 비슷함.
        public event clientService service;

        public Client(int id)
        {
            this.id = id;
        }
        public int Id
        {
            get { return id; }
        }
        public void fireEvent()
        {
            if (service != null)
            {
                EventArgs args = new EventArgs();
                service(this, args);
            }
        }
    }
    class Program
    {
        public static void OnEvent(object sender, EventArgs args)
        {
            Client c = (Client)sender;
            Console.WriteLine("369게임:{0}", c.Id); //WriteLine 개행.
           //Console.Write("369게임:{0}", c.Id); //개행을 사용하지 않음.

            string temp;
           //c.Id.ToString();//Tostring은 메소드라서 마지막에 () 값.
            temp = c.Id.ToString();

            for(int i=0; i<temp.Length; i++)//Length 길이.
            {
                if(temp.Substring(i,1).Equals("3")) //c#에서 비교문 Equals. //메소드이다. 괄호안에 비교할 값을 넣어준다. Substring 한개씩만 비교
                {
                    Console.WriteLine("짝");
                }

                if (temp.Substring(i, 1).Equals("6"))
                {
                    Console.WriteLine("짝");
                }

                if (temp.Substring(i, 1).Equals("9"))
                {
                    Console.WriteLine("짝");
                }
            }
        }

        //Tostring : string으로 변환시켜줌.

        static void Main(string[] args)
        {
            for(int i=1; i<=100; i++)
            {
                Client a = new Client(i);
                a.service += new Client.clientService(OnEvent);
                a.fireEvent();
            }

            Console.ReadKey();
        }
    }
}
        /*
                           class Program
                           {
                               public delegate void myDelegate(string s);

                               public static void test(string s)
                               {
                                   Console.WriteLine(s);
                               }

                               public static void test2(string s)
                               {
                                   Console.WriteLine("test2 : " + s);
                               }
                               static void Main(string[] args)
                               {
                                   myDelegate d = new myDelegate(test);
                                   d("aaa");
                                   d("bbb");
                                   d("ccc");

                                   myDelegate d2 = new myDelegate(test2);
                                   d2("aaa");
                                   d2("bbb");

                                   Console.ReadKey(); //C#의 ("pause")
                               }
                           }
                           */

        /*
        class Program
        {
            static void Main(string[] args)
            {
                Hashtable ht = new Hashtable();
                ht["name"] = "aaa";
                ht["tel"] = "1234";
                ht["address"] = "asdfsd";

                Console.WriteLine(ht["name"]);
                Console.WriteLine(ht["1234"]);
                Console.WriteLine(ht["asdfsd"]);
            }
        }
        */

        /*
    class Program
    {
        static void Main(string[] args)
        {
            ArrayList al = new ArrayList(); //배열의 확장칸. 길이가 정해져있지 않다. 객체파일이면 무엇이든 저장이 가능.
            al.Add("aaa");
            al.Add("bbb");
            al.Add("ccc");
            al.Add("ddd");
            foreach (string s in al)
            {
                Console.WriteLine(s + "/");
            }
            Console.WriteLine();
            al.RemoveAt(1);

            foreach (string s in al)
            {
                Console.WriteLine(s + "/");
            }
            Console.WriteLine();
            Console.WriteLine("데이터 개수 : " + al.Count);
            al.Insert(1, "eee");
            if (al.Contains("ccc"))
            {
                Console.WriteLine(al.IndexOf("ccc") + "번째 방에 있다");
            }
            else
            {
                Console.WriteLine("없다");
            }

            Console.ReadKey(); //C#의 ("pause")
        }
        */

        /*
        class Test
        {
            public static int a;
            public int b;
            public static int cnt;

            public void add()
            {
                a++;
                b++;
            }

            public override string ToString()
            {
                return "a : " + a + ".b :" + b;
            }

            public static void test()
            {
                Console.WriteLine(a);
                // Console.WriteLine(b); //일반 멤버변수 사용안됨.
                test2();
                test3();
            }

            public static void test2()
            {
                Console.WriteLine("test2");
            }

            public static void test3()
            {
                Console.WriteLine("test3");
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                Test t1= new Test();
                t1.add();
                Console.WriteLine(t1);

                Test t2 = new Test();
                t2.add();
                Console.WriteLine(t2);

                Test t3 = new Test();
                t3.add();
                Console.WriteLine(t3);

                Console.WriteLine(Test.a);
                Test.test();

                Console.ReadKey(); //C#의 ("pause")
            }
        }
        */

        /*
        interface Dao
        {
            void insert();
            void select();
            void update();
            void delete();
        }

        class DeolmplOracle : Dao
        {
            public void insert()
            {
                Console.WriteLine("oracle insert");
            }
            public void select()
            {
                Console.WriteLine("oracle select");
            }
            public void update()
            {
                Console.WriteLine("oracle update");
            }
            public void delete()
            {
                Console.WriteLine("oracle delete");
            } 
        }

        class DaolmplMySQL : Dao
        {
            public void insert()
            {
                Console.WriteLine("MySQL insert");
            }
            public void select()
            {
                Console.WriteLine("MySQL select");
            }
            public void update()
            {
                Console.WriteLine("MySQL update");
            }
            public void delete()
            {
                Console.WriteLine("MySQL delete");
            }
        }

        class Service
        {
            private Dao dao;

            public Service(Dao dao)
            {
                this.dao = dao;
            }

            public void addMember()
            {
                dao.insert();
            }
            public void getMember()
            {
                dao.select();
            }

            public void editMember()
            {
                dao.update();
            }

            public void delMember()
            {
                dao.delete();
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                Service s = new Service(new DaolmplMySQL());
                s.addMember();
                s.getMember();
                s.editMember();
                s.delMember();

                Console.ReadKey(); //C#의 ("pause")
            }
        }
        */

        /*
        interface A
        {
            void test1(); //public abstract 생략됨
            void test2(); //public abstract 생략됨
        }

        class ImplA : A
        {
            public void test1()
            {
                Console.WriteLine("ImplA에서 구현한 test1");
            }

            public void test2()
            {
                Console.WriteLine("ImplA에서 구현한 test2");
            }
        }
        class Program
        {
            static void Main(string[] args)
            {
                ImplA a = new ImplA();
                a.test1();
                a.test2();

                Console.ReadKey(); //C#의 ("pause")
            }
        }
        */

        /*
        abstract class GrandParent
        {
            public abstract void test1();
            public abstract void test2();
            public void test3()
            {
                Console.WriteLine("GrandParent test3");
            }
        }
        abstract class Parent : GrandParent
        {
            public override void test1()
            {
                Console.WriteLine("Parent test1");
            }
        }

        class Child : Parent
        {
            public override void test2()
            {
                Console.WriteLine("Child test2");
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                Child c = new Child();
                c.test1();
                c.test2();
                c.test3();

                Console.ReadKey(); //C#의 ("pause")
            }
        }
        */



