import java.io.File
import java.io.RandomAccessFile
import java.nio.ByteBuffer
import java.util.*

fun Double.format(digits: Int) = "%.${digits}f".format(this)

fun main(){
    File(Sorting.LIST_FILE).delete()
    File(Sorting.ARRAY_FILE).delete()
    val counts = arrayOf(100, 200, 400)//, 800, 1600, 3200, 6400)
    var array: Array<DataObject?>
    var list: LinkedList<DataObject?>

    println("".padStart(7 )+ "Array".padStart(14) + "List".padStart(14)
            + "File array".padStart(14) + "File list".padStart(14))
    println()
    
    for(i in counts.indices) {
        print("${counts[i]}:".padStart(7))
        array = Data.generateRandomArray(counts[i])
        Sorting.saveArray(array, Sorting.ARRAY_FILE)
        list = Data.generateRandomList(counts[i])
        Sorting.saveList(list, Sorting.LIST_FILE)

        print("${Sorting.quickSort(Type.ARRAY, array).format(7)}s".padStart(14))

        print("${Sorting.quickSort(Type.FILE_ARRAY, Sorting.ARRAY_FILE).format(7)}s".padStart(14))

        print("${Sorting.quickSort(Type.LIST, list).format(7)}s".padStart(14))

        println("${Sorting.quickSort(Type.FILE_LIST, Sorting.LIST_FILE).format(7)}s".padStart(14))
    }

    array = Data.generateRandomArray(400)
    Sorting.saveArray(array, Sorting.ARRAY_FILE)
    list = Data.generateRandomList(400)
    Sorting.saveList(list, Sorting.LIST_FILE)
    Data.printArray("Unsorted array", array, 250, 260)
    Sorting.quickSort(Type.ARRAY, array)
    Sorting.quickSort(Type.FILE_ARRAY, Sorting.ARRAY_FILE)
    Data.printArray("Sorted array", array, 250, 260)
    Data.printArray("Sorted file array", Sorting.readArrayFile(Sorting.ARRAY_FILE), 250, 260)
    Data.printList("Unsorted list", list, 250, 260)
    Sorting.quickSort(Type.LIST, list)
    Sorting.quickSort(Type.FILE_LIST, Sorting.LIST_FILE)
    Data.printList("Sorted list", list, 250, 260)
    Data.printList("Sorted file list", Sorting.readListFile(Sorting.LIST_FILE), 250, 260)
}

@Suppress("UNCHECKED_CAST")
class Sorting {

    companion object{

        const val ARRAY_FILE = "./array.txt"
        const val LIST_FILE = "./list.txt"

        fun quickSort(type: Type, arg: Any) : Double{
            val t0 = System.nanoTime()
            when(type) {
                Type.ARRAY -> quickSort(type, arg, 0,
                        (arg as Array<DataObject>).lastIndex)
                Type.LIST -> quickSort(type, arg, 0,
                        (arg as LinkedList<DataObject>).lastIndex)
                Type.FILE_ARRAY -> quickSort(type, arg, 0,
                        (File(arg as String).length()/12).toInt()-1)
                Type.FILE_LIST -> quickSort(type, arg, 0,
                        (File(arg as String).length()/16).toInt()-1)
            }
            val t1 = System.nanoTime()
            return (t1-t0)/(1_000_000_000.toDouble())
        }

        private fun quickSort(type: Type, arg: Any, startIndex: Int, endIndex: Int){    // Kaina | Kiekis
            if(startIndex >= endIndex)                                                  // c     | 1
                return
                                                                                        // n = endIndex - startIndex - 1
            val index = when(type) {                                                    // c     | 1
                Type.ARRAY -> partition(type, arg as Array<DataObject?>,
                        startIndex, endIndex)                                       // c + Tp(n) | 1
                Type.LIST -> partition(type, arg as LinkedList<DataObject?>,
                        startIndex, endIndex)
                else -> partition(type, arg, startIndex, endIndex)
            }

                                                                                        // k = n - index - 1
            quickSort(type, arg, startIndex, index-1)                          //       | T(k)
            quickSort(type, arg, index+1, endIndex)                           //        | T(n-k-1)
        } // T(n) = T(k) + T(n-k-1) + 3c + Tp(n)*c = T(k) + T(n-k-1) + O(n)

        private fun partition(type: Type, arg: Any,
                              startIndex: Int, endIndex: Int) : Int {                   // Kaina | Kiekis
            var pivotIndex = startIndex                                                 // c     | 1
            val pivotValue = get(type, arg, endIndex)                               // c + Tget  | 1
                                                                                        // m = endIndex - startIndex
            for(i in startIndex..endIndex){                                             // c     | m + 1
                if(get(type, arg, i) < pivotValue){                              // c + c + Tget | m
                    swap(type, arg, i, pivotIndex)                                  // c + Tswap | m
                    pivotIndex++                                                        // c     | m
                }
            }
            swap(type, arg, pivotIndex, endIndex)                                       // c     | 1
            return pivotIndex                                                           // c     | 1
        } // Tp(m) = 4c + Tget + (m+1)(5c+Tget+Tswap) = 6c + (m+1)c + m(20c) = 21c*m + 7c = O(m)

        fun swap(type: Type, arg: Any, a: Int, b: Int){                                 // Kaina | Kiekis
            if(a==b)                                                                    // c     | 1
                return

            val temp = get(type, arg, a)                                             // c + Tget | 1
            val temp2 = get(type, arg, b)                                            // c + Tget | 1

            set(temp2, type, arg, a)                                                 // c + Tset | 1
            set(temp, type, arg, b)                                                  // c + Tset | 1
        } // Tswap = 5c + 2Tget + 2Tset = 13c

        fun get(type: Type, arg: Any, i: Int) : DataObject {                            // Kaina | Kiekis
            return when(type){                                                          // c     | 1
                Type.ARRAY -> (arg as Array<DataObject>)[i]                             // c     | 1
                Type.LIST -> (arg as LinkedList<DataObject>)[i]
                Type.FILE_ARRAY -> readArrayElement(i, arg as String)
                Type.FILE_LIST -> readListElement(i, arg as String)
            }
        } // Tget = 2c

        private fun set(elem: DataObject, type: Type, arg: Any, i: Int) {                // Kaina | Kiekis
            when(type){                                                                  // c     | 1
                Type.ARRAY -> (arg as Array<DataObject>)[i] = elem                       // c     | 1
                Type.LIST -> (arg as LinkedList<DataObject>)[i] = elem
                Type.FILE_ARRAY -> saveArrayElement(elem, arg as String, i)
                Type.FILE_LIST -> saveListElement(elem, arg as String, i)
            }
        } // Tset = 2c

        fun saveArrayElement(elem: DataObject, file: String, i: Int){
            val raf = RandomAccessFile(file, "rw")
            raf.seek(i*12L)
            raf.write(elem.to12Bytes())
            raf.close()
        }

        fun saveListElement(elem: DataObject, file: String, i: Int){
            val index = if(i == 0) 0 else readListElement(i-1, file).pointer

            val raf = RandomAccessFile(file, "rw")
            raf.seek(index*16L)
            raf.write(elem.to12Bytes())
            raf.close()
        }

        fun readArrayElement(i : Int, file: String) : DataObject{
            val raf = RandomAccessFile(file, "r")
            raf.seek(i*12L)
            val bytes = ByteArray(12)
            raf.read(bytes)
            raf.close()
            val string = String(bytes.copyOfRange(0,8), Charsets.UTF_16)
            val float = ByteBuffer.wrap(bytes.copyOfRange(8,12)).float
            return DataObject(i, string, float)
        }

        fun readListElement(i : Int, file: String) : DataObject{
            val raf = RandomAccessFile(file, "r")
            var string = ""
            var float = 0f
            var pointer = 0
            var j = 0
            do{
                raf.seek(pointer * 16L)
                val bytes = ByteArray(16)
                raf.read(bytes)
                string = String(
                        bytes.copyOfRange(0, 8),
                        Charsets.UTF_16)
                float = ByteBuffer.wrap(
                        bytes.copyOfRange(8, 12)).float
                pointer = ByteBuffer.wrap(
                        bytes.copyOfRange(12, 16)).int
                j++
            } while(i >= j)
            raf.close()
            return DataObject(pointer, string, float)
        }

        fun saveArray(array: Array<DataObject?>, file: String){
            val raf = RandomAccessFile(file, "rw")
            for(i in 0 until array.size) {
                raf.seek(i*12L)
                raf.write(array[i]!!.to12Bytes())
            }
            raf.close()
        }

        fun saveList(list: LinkedList<DataObject?>, file: String){
            val raf = RandomAccessFile(file, "rw")
            for(i in 0 until list.size) {
                raf.seek(i*16L)
                raf.write(list[i]!!.to16Bytes())
            }
            raf.close()
        }

        fun readArrayFile(file: String) : Array<DataObject?> {
            val raf = RandomAccessFile(file, "r")
            val length = (raf.length()/12L).toInt()
            raf.close()
            val array = arrayOfNulls<DataObject>(length)

            for(i in 0 until length){
                array[i] = get(Type.FILE_ARRAY, file, i)
            }

            return array
        }

        fun readListFile(file: String) : LinkedList<DataObject?> {
            val raf = RandomAccessFile(file, "r")
            val length = (raf.length()/16L).toInt()
            raf.close()
            val list = LinkedList<DataObject?>()

            for(i in 0 until length){
                list.add(get(Type.FILE_LIST, file, i))
            }

            return list
        }
    }
}