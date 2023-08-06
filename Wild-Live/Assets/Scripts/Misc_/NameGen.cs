using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGen : MonoBehaviour
{
    [SerializeField] private int nameCount = 1;
    [SerializeField] private int maxNameLength = 50;
    //[SerializeField] private Texture2D texture = null;

    private string[] names = new string[]
    {
        "Olivia",
        "Emma",
        "Charlotte",
        "Amelia",
        "Sophia",
        "Isabella",
        "Ava",
        "Mia",
        "Evelyn",
        "Luna",
        "Harper",
        "Camila",
        "Sofia",
        "Scarlett",
        "Elizabeth",
        "Eleanor",
        "Emily",
        "Chloe",
        "Mila",
        "Violet",
        "Penelope",
        "Gianna",
        "Aria",
        "Abigail",
        "Ella",
        "Avery",
        "Hazel",
        "Nora",
        "Layla",
        "Lily",
        "Aurora",
        "Nova",
        "Ellie",
        "Madison",
        "Grace",
        "Isla",
        "Willow",
        "Zoe",
        "Riley",
        "Stella",
        "Eliana",
        "Ivy",
        "Victoria",
        "Emilia",
        "Zoey",
        "Naomi",
        "Hannah",
        "Lucy",
        "Elena",
        "Lillian",
        "Maya",
        "Leah",
        "Paisley",
        "Addison",
        "Natalie",
        "Valentina",
        "Everly",
        "Delilah",
        "Leilani",
        "Madelyn",
        "Kinsley",
        "Ruby",
        "Sophie",
        "Alice",
        "Genesis",
        "Claire",
        "Audrey",
        "Sadie",
        "Aaliyah",
        "Josephine",
        "Autumn",
        "Brooklyn",
        "Quinn",
        "Kennedy",
        "Cora",
        "Savannah",
        "Caroline",
        "Athena",
        "Natalia",
        "Hailey",
        "Aubrey",
        "Emery",
        "Anna",
        "Iris",
        "Bella",
        "Eloise",
        "Skylar",
        "Jade",
        "Gabriella",
        "Ariana",
        "Maria",
        "Adeline",
        "Lydia",
        "Sarah",
        "Nevaeh",
        "Serenity",
        "Liliana",
        "Ayla",
        "Everleigh",
        "Raelynn",
        "        Olivia",
        "Emma",
        "Charlotte",
        "Amelia",
        "Sophia",
        "Isabella",
        "Ava",
        "Mia",
        "Evelyn",
        "Luna",
        "Harper",
        "Camila",
        "Sofia",
        "Scarlett",
        "Elizabeth",
        "Eleanor",
        "Emily",
        "Chloe",
        "Mila",
        "Violet",
        "Penelope",
        "Gianna",
        "Aria",
        "Abigail",
        "Ella",
        "Avery",
        "Hazel",
        "Nora",
        "Layla",
        "Lily",
        "Aurora",
        "Nova",
        "Ellie",
        "Madison",
        "Grace",
        "Isla",
        "Willow",
        "Zoe",
        "Riley",
        "Stella",
        "Eliana",
        "Ivy",
        "Victoria",
        "Emilia",
        "Zoey",
        "Naomi",
        "Hannah",
        "Lucy",
        "Elena",
        "Lillian",
        "Maya",
        "Leah",
        "Paisley",
        "Addison",
        "Natalie",
        "Valentina",
        "Everly",
        "Delilah",
        "Leilani",
        "Madelyn",
        "Kinsley",
        "Ruby",
        "Sophie",
        "Alice",
        "Genesis",
        "Claire",
        "Audrey",
        "Sadie",
        "Aaliyah",
        "Josephine",
        "Autumn",
        "Brooklyn",
        "Quinn",
        "Kennedy",
        "Cora",
        "Savannah",
        "Caroline",
        "Athena",
        "Natalia",
        "Hailey",
        "Aubrey",
        "Emery",
        "Anna",
        "Iris",
        "Bella",
        "Eloise",
        "Skylar",
        "Jade",
        "Gabriella",
        "Ariana",
        "Maria",
        "Adeline",
        "Lydia",
        "Sarah",
        "Nevaeh",
        "Serenity",
        "Liliana",
        "Ayla",
        "Everleigh",
        "Raelynn",
    };

    [SerializeField]
    private void Start()
    {
        for (int i = 0; i < names.Length; i++) names[i] = names[i].ToLower();

        //var progress = x/_resolution;
        //var height = texture.GetPixel((int)(texture.texelSize.x * progress), (int)(texture.texelSize.y * progress)).r;

        for (int i = 0; i < nameCount; i++)
        {
            Debug.Log(GenerateName());
        }
    }

    private string GenerateName()
    {
        var name = new List<char>();
        var currentIdx = 0;

        for(int i = 0; i < maxNameLength; i++)
        {
            var possibleName = string.Empty;

            if (currentIdx == 0)
            {
                name.Add(names[Random.Range(0, names.Length)][0]);
                currentIdx++;
                continue;
            }
            else if (currentIdx == 1)
            {
                var possibleNames = names.Where(name => name.Contains(name[0])).ToArray();
                possibleName = possibleNames[Random.Range(0, possibleNames.Length)];
            }
            else
            {
                var possibleNames = names.Where(name => name.Contains($"{name[currentIdx - 2]}{name[currentIdx - 1]}")).ToArray();
                if (possibleNames.Length == 0) break;
                possibleName = possibleNames[Random.Range(0, possibleNames.Length)];
            }
            var idx = possibleName.IndexOf(name[0]);
            if (idx >= possibleName.Length - 1) break;
            else name.Add(possibleName[idx + 1]);

            currentIdx++;
        }
        return new string(name.ToArray());
    }
}
