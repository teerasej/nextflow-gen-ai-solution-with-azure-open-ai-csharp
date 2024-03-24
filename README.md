
# Building Generative AI Solution for Developer

> ต้องมีการติดตั้ง .NET SDK และ Visual Studio Code ก่อนใช้งานให้เรียบร้อย ดาวน์โหลดได้ที่ [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download) และ [https://code.visualstudio.com/](https://code.visualstudio.com/)

1. ดาวน์โหลด zip ไฟล์ และแตกไฟล์
2. เปิดโฟลเดอร์โปรเจคขึ้นใมาใน Visual Studio Code

## Setup environment

3. เปลี่ยนชื่อไฟล์ `env-temp` เป็น `.env`
4. ใส่ Azure OpenAI Key และ URL ที่ได้มา ลงไปในไฟล์ `.env` และบันทึกไฟล์

```
AZURE_OPENAI_API_KEY=XXXXXXXXXXXXXXX
AZURE_OPENAI_URL=XXXXXXXXXXXXXXX
```

## 1. Create a new OpenAIClient instance with the endpoint and AzureKeyCredential.

5. เปิดไฟล์ `Program.cs` 
6. ให้แน่ใจว่าแทนที่ URL ด้วย URL ของ Azure OpenAI API 

```csharp
OpenAIClient client = new OpenAIClient(
  new Uri(Environment.GetEnvironmentVariable("AZURE_OPENAI_URL")),
  new AzureKeyCredential(Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY")));
```

## 2. Read the user input from the console.

ในส่วนที่ 2 เรามีการสร้างการรับข้อมูลจาก Console โดยใช้ `Console.ReadLine()` และสร้าง ChatMessage object โดยใช้ค่าจาก input และระบุ ChatRole เป็น User

```csharp
Console.WriteLine("Enter your message: ");
string input = Console.ReadLine();

// สร้าง ChatMessage object โดยใช้ค่าจาก input และระบุ ChatRole เป็น User
var newChatMessage = new ChatMessage(ChatRole.User, input);
```

## 3. Create a new ChatMessage instance with the user input.

ในส่วนที่ 3 เรามีการเรียกใช้ `GetChatCompletionsAsync` โดยส่งข้อมูลที่ได้รับจากผู้ใช้เข้าไปในข้อมูลที่ส่งให้ API และระบุค่าต่างๆที่ต้องการให้ API ใช้ในการสร้างข้อความตอบกลับ

```csharp
// 3. Create a new ChatMessage instance with the user input.
Response<ChatCompletions> responseWithoutStream = await client.GetChatCompletionsAsync(
"gpt-4-32k",
new ChatCompletionsOptions()
{
  Messages =
  {
      // System Message
      new ChatMessage(ChatRole.System, @"You are an AI assistant that helps people extract sentiment from the message. Score between 0.0-1.0. Output score and a polite response message that includes the summary of the problem."),

      // Few shot example
      new ChatMessage(ChatRole.User, @"วันนี้มีผู้ป่วยเยอะ ทำให้โรงพยาบาลแจกยาช้า และฉันต้องยืน เพราะไม่มีเก้าอี้เหลือให้นั่ง"),      new ChatMessage(ChatRole.Assistant, @"{ ""score"": 0.3, ""response"": ""ขออภัยในความไม่สะดวกที่เกิดขึ้นนะคะ เราจะปรับปรุงการจัดการผู้ป่วยในวันที่คนเยอะ และเพิ่มจำนวนเก้าอี้ให้นั่งให้กับผู้ป่วยค่ะ ขอบคุณที่ให้ข้อมูลเพื่อการปรับปรุงค่ะ"" }"), 
      
      // เพิ่ม ChatMessage ที่ระบุ ChatRole เป็น User และใส่ข้อความที่ได้รับจากผู้ใช้เข้าไปในข้อมูลที่ส่งให้ API
      newChatMessage
  },
  // ระบุค่าต่างๆที่ต้องการให้ API ใช้ในการสร้างข้อความตอบกลับ
  Temperature = (float)0.7,
  MaxTokens = 800,
  NucleusSamplingFactor = (float)0.95,
  FrequencyPenalty = 0,
  PresencePenalty = 0,
});
```

## 4. Call the GetChatCompletionsAsync method with the GPT-4 model and ChatCompletionsOptions.

ส่วนที่ 4 เราจะมีการรอรับข้อมูลจาก API โดยใช้ `responseWithoutStream.Value` และเก็บข้อมูลที่ได้ไว้ในตัวแปรชื่อ `response`

```csharp
ChatCompletions response = responseWithoutStream.Value;
```

## 5. Print the response message from the assistant.

ส่วนสุดท้ายเราจะมีการนำข้อความที่ได้จาก API มาแสดงผลบน Console

```csharp
Console.WriteLine(response.Choices[0].Message.Content);
```