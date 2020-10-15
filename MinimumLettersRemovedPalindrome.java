import java.util.*;

public class MinimumLettersRemovedPalindrome {

    public static void main(String[] args) {
        String[] test = { "aaba", "asddd", "daabd", "racecar", "aebcbda", "testing" };
        for (String arg : test) {
            System.out.printf("For \"%s\" to become a palindrome, we need to remove %d characters %n", arg,
                    minimumRemoved(arg));
        }
    }

    public static int minimumRemoved(String str) {
        int length = str.length();

        Map<Character, Integer> charCounts = characterCounts(str);

        boolean hasOdds = false;
        int totalCount = 0;
        // Remove a character from all but one odd counted character
        for (Map.Entry<Character, Integer> s : charCounts.entrySet()) {
            int count = s.getValue();
            totalCount += count;
            if (count % 2 != 0) {
                if (hasOdds) {
                    charCounts.put(s.getKey(), count - 1);
                    totalCount--;
                } else
                    hasOdds = true;
            }
        }
        return length - totalCount;
    }

    // returns a hashmap with characters and their counts from given string
    public static Map<Character, Integer> characterCounts(String str) {
        HashMap<Character, Integer> charCounts = new HashMap<Character, Integer>();
        for (int i = 0; i < str.length(); i++) {
            char c = str.charAt(i);
            Integer count = charCounts.get(c);
            if (count == null)
                count = 0;
            count++;
            charCounts.put(c, count);
        }
        return charCounts;
    }
}