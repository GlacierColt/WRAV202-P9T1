using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WRAV202_P9T1
{
    internal class Program
    {
        string FILE = "FILE.txt";
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            /*Tree tree = new Tree();
            tree.Root = new Node();
            tree.Root.cargo = '&';
            Node temp = new Node();

            //Right
            temp = tree.Root;
            temp.right = new Node();
            temp.right.cargo = '&';
            temp = temp.right;
            temp.left = new Node();
            temp.left.cargo = 'F';
            temp.right = new Node();
            temp.right.cargo = 'T';

            //Left
            temp = tree.Root;
            temp.left = new Node();
            temp.left.cargo = '|';
            temp = temp.left;
            temp.right = new Node();
            temp.right.cargo = 'F';
            temp.left = new Node();
            temp.left.cargo = '|';
            temp = temp.left;
            temp.left = new Node();
            temp.left.cargo = 'T';
            temp.right = new Node();
            temp.right.cargo = 'F';*/

            TreeNode temp1 = new OperatorNode(new ValNode(true), '|', new ValNode(false));
            TreeNode temp2 = new OperatorNode(temp1, '|' , new ValNode(false));
            TreeNode temp3 = new OperatorNode(new ValNode(false), '&', new ValNode(true));
            TreeNode root = new OperatorNode(temp2, '&', temp3);

            Console.Write("Enter file name: ");
            string filePath = Console.ReadLine();

            SaveTreeToFile(root, filePath);

            Console.WriteLine(root.Evaluate().ToString());
            Console.ReadLine();



        }

       
        public void SaveTreeToFile(TreeNode root, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                SerializeTree(root, writer);
            }
        }

        public TreeNode LoadTreeFromFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                return DeserializeTree(reader);
            }
        }

        private void SerializeTree(TreeNode node, StreamWriter writer)
        {
            if (node is ValNode valNode)
            {
                writer.WriteLine("ValueNode:" + valNode.value);
            }
            else if (node is OperatorNode opNode)
            {
                writer.WriteLine("OperatorNode:" + opNode.opera);
                SerializeTree(opNode.left, writer);
                SerializeTree(opNode.right, writer);
            }
        }

        private TreeNode DeserializeTree(StreamReader reader)
        {
            string line = reader.ReadLine();
            if (line != null)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string nodeType = parts[0];
                    string nodeValue = parts[1];

                    if (nodeType == "ValueNode")
                    {
                        if (bool.TryParse(nodeValue, out bool value))
                        {
                            return new ValNode(value);
                        }
                    }
                    else if (nodeType == "OperatorNode")
                    {
                        char oper = char.Parse(nodeValue);
                        TreeNode left = DeserializeTree(reader);
                        TreeNode right = DeserializeTree(reader);
                        return new OperatorNode(left, oper, right);
                    }
                }
            }

            return null;
        }
    }

    class ValNode : TreeNode
    {
        public bool value;

        public ValNode(bool v)
        {
            value = v;
        }

        public override bool Evaluate()
        {
            return value;
        }

    }

    class OperatorNode : TreeNode
    {
        public TreeNode left;
        public TreeNode right;
        public char opera;

        public OperatorNode(TreeNode l, char oper, TreeNode r)
        {
            left = l;
            right = r;
            opera = oper;
        }

        

        public override bool Evaluate()
        {
            switch (opera)
            {
                case '&': return (left.Evaluate() && right.Evaluate());
                case '|': return (left.Evaluate() || right.Evaluate());
                default: return false;
            }

        }

    }

    class TreeNode
    {
        public virtual bool Evaluate()
        {
            return false;
        }
    }
}
