
// Note: The Azure OpenAI client library for .NET is in preview.
// Install the .NET library via NuGet: dotnet add package Azure.AI.OpenAI --version 1.0.0-beta.5
using Azure;
using Azure.AI.OpenAI;
using dotenv.net;

DotEnv.Load();

// 1. Create a new OpenAIClient instance with the endpoint and AzureKeyCredential.



// 2. Read the user input from the console.
Console.WriteLine("Enter your message: ");
string input = Console.ReadLine();

// สร้าง ChatMessage object โดยใช้ค่าจาก input และระบุ ChatRole เป็น User



// 3. Create a new ChatMessage instance with the user input.
Response<ChatCompletions> responseWithoutStream = await client.GetChatCompletionsAsync(
"gpt-4-32k",
new ChatCompletionsOptions()
{
  Messages =
  {
      new ChatMessage(ChatRole.System, @"You are an AI assistant that helps people extract sentiment from the message. Score between 0.0-1.0. Output score and a polite response message that includes the summary of the problem."),
      new ChatMessage(ChatRole.User, @"วันนี้มีผู้ป่วยเยอะ ทำให้โรงพยาบาลแจกยาช้า และฉันต้องยืน เพราะไม่มีเก้าอี้เหลือให้นั่ง"),      new ChatMessage(ChatRole.Assistant, @"{ ""score"": 0.3, ""response"": ""ขออภัยในความไม่สะดวกที่เกิดขึ้นนะคะ เราจะปรับปรุงการจัดการผู้ป่วยในวันที่คนเยอะ และเพิ่มจำนวนเก้าอี้ให้นั่งให้กับผู้ป่วยค่ะ ขอบคุณที่ให้ข้อมูลเพื่อการปรับปรุงค่ะ"" }"), 
      
      // เพิ่ม ChatMessage ที่ระบุ ChatRole เป็น User และใส่ข้อความที่ได้รับจากผู้ใช้เข้าไปในข้อมูลที่ส่งให้ API
     
  },
  // ระบุค่าต่างๆที่ต้องการให้ API ใช้ในการสร้างข้อความตอบกลับ
  Temperature = (float)0.7,
  MaxTokens = 800,
  NucleusSamplingFactor = (float)0.95,
  FrequencyPenalty = 0,
  PresencePenalty = 0,
});


// 4. Call the GetChatCompletionsAsync method with the GPT-4 model and ChatCompletionsOptions.


// 5. Print the response message from the assistant.