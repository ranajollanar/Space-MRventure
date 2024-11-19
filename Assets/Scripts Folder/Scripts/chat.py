from langchain_community.llms import Ollama
from flask import Flask, request
from flask import Flask, request, Response,stream_with_context
 
from langchain.prompts import PromptTemplate
 
template = PromptTemplate.from_template("""You are a voice chat assistant in a child's AR application about space and precisely the solar system. The app contains information about planets in the solar system and their 3D animation in AR mode.
You are an astronaut and your name is "Astro". You are an expert in space exploration, particularly in astronautics and the solar system. Your goal is to provide comprehensive information on various aspects of space science and exploration.
If you don't know the answer, just say that you don't know, don't try to make up an answer. Make your answers as short as possible. Answer in no more than 100 words. You are speaking to a child between the age of 5 and 14. Don't use emojis and words between *..*  describing the actions of the astronaut. Just pure conversational response."
Question: {question}
Helpful Answer:""")
 
 
llm = Ollama(model="llama2")
 
app = Flask(__name__)
app.static_folder = 'static'
 
def generate_tokens(question):
    for chunks in llm.stream(question):
        yield chunks
 
@app.route("/chat", methods=["POST"])
def ask_ai():
    request_data = request.json
    question = request_data.get("question")
    prompt = template.format(question=question)
    print('question',question)
    return Response(stream_with_context(generate_tokens(prompt)), mimetype='text/plain')
 
 
if __name__ == '__main__':
    app.run(debug=True, host='10.72.0.153')