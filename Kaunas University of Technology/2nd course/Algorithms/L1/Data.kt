import sun.awt.image.ImageWatched
import java.util.*

class Data {
    companion object{
        private const val sourceString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzĄČĘĖĮŠŲŪŽąčęėįšųūž"

        fun printArray(array: Array<DataObject?>, from: Int, to: Int){
            printArray("Printing array", array, from, to)
        }
        fun printArray(title: String, array: Array<DataObject?>, from: Int, to: Int){
            println("*** $title ***")
            for(i in from..to){
                println("${"[$i]".padStart(7)}: " + array[i])
            }
            println("**********************")
        }

        fun printList(list: LinkedList<DataObject?>, from: Int, to: Int){
            printList("Printing list", list, from, to)
        }
        fun printList(title: String, list: LinkedList<DataObject?>, from: Int, to: Int){
            println("*** $title ***")
            for(i in from..to){
                println("${"[$i]".padStart(7)}: ${list[i]}")
            }
            println("**********************")
        }


        fun generateRandomArray(size: Int) : Array<DataObject?> {
            val array = arrayOfNulls<DataObject>(size)

            val random = Random(System.currentTimeMillis())

            for(i in 0 until size){
                var string = ""
                for(j in 0 until 4){
                    string += sourceString[random.nextInt(sourceString.length)]
                }
                array[i] = DataObject(i, string, random.nextFloat())
            }

            return array
        }
        fun generateRandomList(size: Int) : LinkedList<DataObject?>{
            val list = LinkedList<DataObject?>()
            val random = Random(System.currentTimeMillis())

            for(i in 0 until size){
                var string = ""
                for(j in 0 until 4){
                    string += sourceString[random.nextInt(sourceString.length)]
                }
                list.add(DataObject(i+1, string, random.nextFloat()))
            }

            return list
        }
    }
}