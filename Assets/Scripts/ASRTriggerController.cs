using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class CleverbotResponse
{
    public string cs;
    public string output;
}

public class CharadesQuestion
{
    public string solution;
    public string[] hints;
}

public class ASRTriggerController : MonoBehaviour
{
    public Material materialDGRed;
    public Material materialApple;
    public Material materialButterfly;
    public Material materialCaterpillar;

    public Material materialDGversalStudios;
    public Material materialDGversity;
    public Material materialDGverse;

    bool isButterfly = false;

    private bool cleverbotConversationStarted = false;
    private string cleverbotCS;

    private bool askingQuestion = false;
    private int hintIndex = 0;
    private int questionIndex = 0;
    private CharadesQuestion[] charadeQuestions = new CharadesQuestion[] {
        new CharadesQuestion {
            solution = "zoo",
            hints = new string [] {
                "Dr. Seuss wanted to run one",
                "It might be filled with lions, tigers, and bears, oh my",
                "The national one has a panda cam",
                "It's a popular field trip spot",
                "It might be the only place you get to see an elephant",
                "It's filled with animals"
            }
        },
        new CharadesQuestion {
            solution = "book",
            hints = new string [] {
                "A judge might throw one at you",
                "Chris and Streeter aren't sure which way is up",
                "It can transport you",
                "You might have cases for them",
                "You probably had to buy some for college classes",
                "It's best when read"
            }
        },
        new CharadesQuestion {
            solution = "apple",
            hints = new string [] {
                "It can be a lady or a granny",
                "It's a treat for teacher",
                "It keeps doctors away",
                "It's delicious in pie & cobbler",
                "You pick it in the fall",
                "It can be Red Delicious"
            }
        },
        new CharadesQuestion {
            solution = "birthday",
            hints = new string [] {
                "Everyone has one",
                "It's a date",
                "It's listed on most forms of ID",
                "It determines your age",
                "It happens once a year",
                "It's traditionally celebrated with cake, candles and presents"
            }
        },
        new CharadesQuestion {
            solution = "arm",
            hints = new string [] {
                "It's the essential part of a baseball pitcher",
                "Your chair might have two",
                "You probably have two too",
                "It has a fore and an upper part",
                "Someone with a broken one gets a cast",
                "It's the body part that connects your wrist and elbow"
            }
        },
        new CharadesQuestion {
            solution = "frog",
            hints = new string [] {
                "It crosses the road in a video game",
                "It leaps",
                "It might be a bull ",
                "It uses its tongue to catch lunch on the fly",
                "It's an amphibian",
                "It's not a toad"
            }
        },
        new CharadesQuestion {
            solution = "balloon",
            hints = new string [] {
                "It floats on a string",
                "It might be filled with gas",
                "You should watch out for the water ones!",
                "Clowns can make animals from them",
                "Sometimes they say Happy Birthday",
                "The make a loud sound when they pop"
            }
        },
        new CharadesQuestion {
            solution = "beach",
            hints = new string [] {
                "It goes with volleyball, ball, and towel",
                "It's a great place to catch some rays",
                "Hotels on this are very expensive!",
                "It's a great place for a sandcastle",
                "The ocean is there",
                "It's sandy"
            }
        },
        new CharadesQuestion {
            solution = "bird",
            hints = new string [] {
                "It has wings",
                "Not all of them fly",
                "It might visit your garden for a snack",
                "It can range in size from just larger than a bumblebee to over 8 feet tall",
                "It evolved from dinosaurs",
                "It hatched from an egg"
            }
        },
        new CharadesQuestion {
            solution = "lollipop",
            hints = new string [] {
                "It might be a Dum Dum",
                "It's a song by the Chordettes, Ben Kweller, and Lil' Wayne",
                "It might be filled with bubble gum or tootsie roll",
                "No one knows how many licks it takes to get to the center ",
                "Some people think they are suckers",
                "It's sugar on a stick"
            }
        },
        new CharadesQuestion {
            solution = "spider",
            hints = new string [] {
                "It might be itsy-bitsy",
                "It goes up the water spout",
                "It might get washed out by rain",
                "One named Boris has his own song",
                "It has eight legs",
                "It's quite the webspinner "
            }
        },
        new CharadesQuestion {
            solution = "truck",
            hints = new string [] {
                "It's a heavy duty hauler",
                "Henry Ford built some",
                "They're big in Texas",
                "They come in super duty and king ranch editions",
                "They have cabs",
                "Silverado is one"
            }
        },
        new CharadesQuestion {
            solution = "boat",
            hints = new string [] {
                "It's a hole in the water",
                "It can have multiple means of propulsion",
                "One method of propulsion might be the wind",
                "Moe spends a lot of spare time working on his",
                "It might have a mast, rigging, and sails",
                "Some people prefer the term ship or vessel"
            }
        },
        new CharadesQuestion {
            solution = "penny",
            hints = new string [] {
                "It's a piece of change",
                "Your thoughts cost one",
                "You can leave one or take one",
                "It's not really copper",
                "Lincoln likes it",
                "It's one cent"
            }
        },
        new CharadesQuestion {
            solution = "baseball",
            hints = new string [] {
                "It's a sport with a bat",
                "You play on a diamond",
                "You can steal in this game",
                "They encourage peanuts and Cracker Jacks with song",
                "Babe Ruth is known to have played",
                "It's name is two of the major components of the game"
            }
        },
        new CharadesQuestion {
            solution = "banana",
            hints = new string [] {
                "It's a yellow fruit",
                "It might be the most common fruit in comedy",
                "It's recommended you peel this one",
                "It gets split with ice cream",
                "Elvis liked his with on a sandwich with peanut butter",
                "Monkeys like this fruit"
            }
        },
        new CharadesQuestion {
            solution = "baby",
            hints = new string [] {
                "You have to be careful, it's fragile",
                "It requires a lot of special equipement",
                "It's day job is to sleep, eat, and cry",
                "It's still growing",
                "Mom and Dad plus this makes three",
                "It's a brand new human"
            }
        },
        new CharadesQuestion {
            solution = "duck",
            hints = new string [] {
                "It's the second D of Dodgeball",
                "Two of them come before goose in a children's game",
                "It's a logo for Dooney & Bourke",
                "They have a hockey team, 3 movies, and a TV show",
                "Hunting them was the focus of a 1984 NES game",
                "Darkwing, Daffy and Donald are some"
            }
        },
        new CharadesQuestion {
            solution = "alligator",
            hints = new string [] {
                "It's a dangerous reptile",
                "It's cousin had a huge roll in Peter Pan",
                "It can be found in Florida ",
                "It lives on land and in the water",
                "It has a big mouth",
                "It's not a crocodile"
            }
        },
        new CharadesQuestion {
            solution = "pants",
            hints = new string [] {
                "Never leave home without them",
                "They are great hiney hiders!",
                "You can fly by the seat of them",
                "The stretchy ones are the best",
                "They come in running, jogger, and sweat varieties",
                "Jeans, slacks, khakis"
            }
        },
        new CharadesQuestion {
            solution = "fire",
            hints = new string [] {
                "3 3s in a row in NBA Jam will light you on this",
                "The Doors wanted you to light it",
                "It might have a place in your home",
                "Your outfit might be this",
                "It's helpful for making s'mores",
                "It's burning up"
            }
        },
        new CharadesQuestion {
            solution = "puppy",
            hints = new string [] {
                "Petting one is the best therapy",
                "It might chew your shoes",
                "It grows up because its just a baby",
                "You have to train it and you might need to teach it to go outside",
                "They have a lot of energy and love to play fetch",
                "It's a baby corgi"
            }
        },
        new CharadesQuestion {
            solution = "big",
            hints = new string [] {
                "The first word in Nelly's song Air Force Ones",
                "You might describe your older brother this way",
                "Like giants",
                "Like skyscrapers",
                "Synonym for huge",
                "Not small"
            }
        },
        new CharadesQuestion {
            solution = "basketball",
            hints = new string [] {
                "The Fresh Prince played outside of school",
                "The goal is 10 feet high",
                "You can travel in this game",
                "The court has keys",
                "It's a sport for tall people",
                "It's also known as hoops"
            }
        },
        new CharadesQuestion {
            solution = "rainbow",
            hints = new string [] {
                "There are riches at the end",
                "Leprechauns like them",
                "It's quite colorful",
                "It has something to do with a guy named ROY",
                "They need a little rain and a little sun to appear",
                "It's the University of Hawaii Mascot"
            }
        },
        new CharadesQuestion {
            solution = "bed",
            hints = new string [] {
                "They come in a Full suite of sizes",
                "Goldilocks was found in Mama Bear's",
                "You shouldn't jump on it",
                "You can get one with water!",
                "You can be served breakfast there ",
                "You probably sleep in one"
            }
        },
        new CharadesQuestion {
            solution = "bike",
            hints = new string [] {
                "You probably know how to ride one",
                "Some roads have lanes just for these",
                "They come in dirt, street, and racing variations",
                "They ride these on a famous tour in France",
                "It's the first word in BMX",
                "They have two wheels"
            }
        },
        new CharadesQuestion {
            solution = "water",
            hints = new string [] {
                "You should drink it",
                "The wellness challenge wants you to drink more",
                "It might kill you if you go too deep",
                "It might kill you if you go too deep",
                "It helps you wash things",
                "70% of your body is made out of it "
            }
        },
        new CharadesQuestion {
            solution = "belt",
            hints = new string [] {
                "Your pants might have loops just for this",
                "Some of them are reversable",
                "You can get a big buckle for it",
                "You car probably has a few",
                "Sour ones are pretty delicious",
                "It holds your pants up"
            }
        },
        new CharadesQuestion {
            solution = "cowboy",
            hints = new string [] {
                "11 are on the field in Dallas",
                "Howdy Doodie, Woody, and The Lone Ranger are some",
                "According to a song, you can save a horse by riding one ",
                "You can find them at the rodeo",
                "They are an essential part of Westerns",
                "A lasso, boots, and a stetson are part of the uniform"
            }
        }
    };

    bool onCharadesPlane = false;
    bool onSentimentPlane = false;

    void Start()
    {
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(2);

        GameObject x = GameObject.CreatePrimitive(PrimitiveType.Cube);
        x.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 8.0f;
        x.transform.localScale = new Vector3(4.0f, 1.0f, 0.1f);
        x.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - x.transform.position);
        x.GetComponent<Renderer>().material = materialDGversalStudios;
        StartCoroutine(PlayTextAsAudio("d g verse all studios presents"));
        yield return new WaitForSeconds(3);
        Rigidbody xRigidbody = x.AddComponent<Rigidbody>();

        GameObject y = GameObject.CreatePrimitive(PrimitiveType.Cube);
        y.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 8.0f;
        y.transform.localScale = new Vector3(4.0f, 1.0f, 0.1f);
        y.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - y.transform.position);
        y.GetComponent<Renderer>().material = materialDGverse;
        StartCoroutine(PlayTextAsAudio("the first installment of the d g verse"));
        yield return new WaitForSeconds(4);
        Rigidbody yRigidbody = y.AddComponent<Rigidbody>();

        GameObject z = GameObject.CreatePrimitive(PrimitiveType.Cube);
        z.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 8.0f;
        z.transform.localScale = new Vector3(4.0f, 1.0f, 0.1f);
        z.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - z.transform.position);
        z.GetComponent<Renderer>().material = materialDGversity;
        StartCoroutine(PlayTextAsAudio("the d g verse city"));
        yield return new WaitForSeconds(3);
        Rigidbody zRigidbody = z.AddComponent<Rigidbody>();
    }

    void Update()
    {
        GameObject charadesPlane = GameObject.Find("CharadesPlane");
        GameObject sentimentPlane = GameObject.Find("SentimentPlane");

        if (Vector3.Distance(Camera.main.transform.position, charadesPlane.transform.position) < 25 * Mathf.Sqrt(2))
        {
            if (!onCharadesPlane)
            {
                onCharadesPlane = true;
                StartCoroutine(PlayTextAsAudio("this is speech charades, you have to guess what I am thinking of based on a series of hints, you can say next to hear the next hint or skip to move on to the next question. say question to begin"));
            }
        }
        else
        {
            onCharadesPlane = false;
            askingQuestion = false;
        }

        if (Vector3.Distance(Camera.main.transform.position, sentimentPlane.transform.position) < 25 * Mathf.Sqrt(2))
        {
            if (!onSentimentPlane)
            {
                onSentimentPlane = true;
                StartCoroutine(PlayTextAsAudio("Hatch the chrysalis by telling it happy stories! Press space on a computer or the ay button on a quest 2 to start recording your story. But be careful! If you get angry or upset, you might kill it!"));
            }
        }
        else
        {
            onSentimentPlane = false;
        }
    }

    public void HandleSentimentASR(Word[] words)
    {
        if (onSentimentPlane)
        {
            int positiveCount = 0;
            int negativeCount = 0;
            foreach (var word in words)
            {
                var sentiment = word.sentiment;
                Debug.Log(sentiment);
                if (sentiment == "positive") positiveCount += 1;
                if (sentiment == "negative") negativeCount += 1;
            }
            Debug.Log("positive: " + positiveCount);
            Debug.Log("negative: " + negativeCount);
            GameObject butterflyCube = GameObject.Find("Butterfly");
            if (positiveCount > negativeCount && !isButterfly)
            {
                isButterfly = true;
                butterflyCube.GetComponent<Renderer>().material = materialButterfly;
            }
            else if (negativeCount > positiveCount && isButterfly)
            {
                isButterfly = false;
                butterflyCube.GetComponent<Renderer>().material = materialCaterpillar;
            }
            else if (negativeCount > positiveCount && !isButterfly)
            {
                Rigidbody butterflyRigidbody = butterflyCube.AddComponent<Rigidbody>();
            }
        }
    }

    public void HandleASR(string message)
    {
        Debug.Log("HandleASR: " + message);

        if (message.Contains("apple"))
        {
            GameObject x = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Rigidbody xRigidbody = x.AddComponent<Rigidbody>();
            x.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 5.0f;
            x.transform.localScale = new Vector3(1.0f, 1.0f, 0.1f);
            x.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - x.transform.position);
            x.GetComponent<Renderer>().material = materialApple;
        }

        GameObject lobbyPlane = GameObject.Find("LobbyPlane");
        GameObject cleverbotPlane = GameObject.Find("CleverbotPlane");

        if (Vector3.Distance(Camera.main.transform.position, lobbyPlane.transform.position) < 25 * Mathf.Sqrt(2))
        {
            if (message.Contains("cube"))
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Rigidbody cubeRigidbody = cube.AddComponent<Rigidbody>();
                cube.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 5.0f;
            }

            if (message.Contains("sphere"))
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Rigidbody sphereRigidbody = sphere.AddComponent<Rigidbody>();
                sphere.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 5.0f;
            }

            if (message.Contains("plane") || message.Contains("plain"))
            {
                GameObject x = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Rigidbody xRigidbody = x.AddComponent<Rigidbody>();
                x.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 5.0f;
                x.transform.localScale = new Vector3(1.0f, 1.0f, 0.1f);
                x.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - x.transform.position);
                x.GetComponent<Renderer>().material = materialDGRed;
            }
        }

        if (onCharadesPlane)
        {
            if (message.Contains(charadeQuestions[questionIndex].solution) && askingQuestion)
            {
                StartCoroutine(PlayTextAsAudio("you are correct, say question to hear the next question"));
                askingQuestion = false;
            }

            if (message.Contains("question") && !askingQuestion)
            {
                hintIndex = 0;
                questionIndex += 1;
                if (questionIndex >= charadeQuestions.Length)
                {
                    questionIndex = 0;
                }
                StartCoroutine(PlayTextAsAudio(charadeQuestions[questionIndex].hints[hintIndex]));
                askingQuestion = true;
            }

            if (message.Contains("next") && askingQuestion)
            {
                hintIndex += 1;
                if (hintIndex >= charadeQuestions[questionIndex].hints.Length)
                {
                    hintIndex = 0;
                }
                StartCoroutine(PlayTextAsAudio(charadeQuestions[questionIndex].hints[hintIndex]));
            }

            if (message.Contains("skip") && askingQuestion)
            {
                hintIndex = 0;
                questionIndex += 1;
                if (questionIndex >= charadeQuestions.Length)
                {
                    questionIndex = 0;
                }
                StartCoroutine(PlayTextAsAudio(charadeQuestions[questionIndex].hints[hintIndex]));
            }
        }

        if (Vector3.Distance(Camera.main.transform.position, cleverbotPlane.transform.position) < 25 * Mathf.Sqrt(2))
        {
            if (message.Length > 0)
            {
                StartCoroutine(GetCleverbotResponse(message));
            }
        }
    }

    IEnumerator GetCleverbotResponse(string text)
    {
        GameObject cleverbot = GameObject.Find("Cleverbot");

        string url = "https://www.cleverbot.com/getreply?key=" + "INSERT_YOUR_CLEVERBOT_API_KEY";
        if (cleverbotConversationStarted)
        {
            url += "&cs=" + cleverbotCS;
        }
        url += "&input=" + text;

        UnityWebRequest uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            CleverbotResponse cleverbotResponse = JsonUtility.FromJson<CleverbotResponse>(uwr.downloadHandler.text);
            StartCoroutine(PlayTextAsAudioAtPosition(cleverbotResponse.output, cleverbot.transform.position));
            cleverbotCS = cleverbotResponse.cs;
            cleverbotConversationStarted = true;
        }
    }

    IEnumerator PlayTextAsAudioAtPosition(string text, Vector3 position)
    {
        string url = "https://dgversetts.deepgram.com/text-to-speech/polly?text=" + text;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                AudioSource.PlayClipAtPoint(myClip, position);
            }
        }
    }

    IEnumerator PlayTextAsAudio(string text)
    {
        string url = "https://dgversetts.deepgram.com/text-to-speech/gtts?text=" + text;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.clip = myClip;
                audioSource.Play();
            }
        }
    }
}