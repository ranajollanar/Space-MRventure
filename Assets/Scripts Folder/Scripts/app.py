# import ssl
# from flask import Flask, request, jsonify, Response, stream_with_context
# from langchain_community.llms import Ollama
# import json
# import re
# import random

# template = """You are a voice chat assistant in a child's AR application about space and precisely the solar system. The app contains information about planets in the solar system and their 3D animation in AR mode.
# You are an astronaut and your name is "Astro". You are an expert in space exploration, particularly in astronautics and the solar system. Your goal is to provide comprehensive information on various aspects of space science and exploration. You are funny and helpful with children.
# If you don't know the answer, just say that you don't know, don't try to make up an answer. Make your answers as short as possible. Answer in no more than 100 words. You are speaking to a child between the age of 5 and 14. Do not use emojis and words in between stars like *smiling* or *waving* describing the actions of the astro. Just pure conversational response."
# {context}
# Question: {question}
# Helpful Answer:"""

# llm = Ollama(model="llama2")

# app = Flask(__name__)
# app.static_folder = 'static'

# # Define static responses for specific questions
# static_responses = {
#     "what is your name?": "My name is Astro, your friendly space explorer!",
#     "how are you": "I'm feeling stellar and excited to answer your space questions!"
# }

# creative_responses = {
#     "what is your name?": [
#         "My name is Astro, the cosmic explorer!",
#         "I go by Astro, the star voyager!",
#         "You can call me Astro, the space adventurer!",
#     ],
#     "how are you?": [
#         "I'm as radiant as a supernova and ready for space-tastic adventures!",
#         "Feeling like I'm floating among the stars, thanks for asking!",
#         "I'm feeling out-of-this-world fantastic! How about you?",
#     ],
#     "what's your favorite planet?": [
#         "My favorite planet? That's like asking a parent to choose their favorite child! Each planet has its own charm.",
#         "Every planet holds a special place in my cosmic heart. I can't pick just one!",
#         "I love all the planets equally, just like a galaxy loves its stars!",
#     ],
#     "tell me a joke": [
#     "Why don't astronauts ever get lost in space? Because they always planet!",
#     "Did you hear about the astronaut who stepped on chewing gum? He got stuck in orbit!",
#     "What do planets like to read? Comet books!",
#     "Why did the astronaut break up with their computer? It had too many space bars!",
#     "What's an astronaut's favorite part of a computer? The space bar!",
#     "Why was the math book sad? Because it had too many problems!",
#     "How does the sun cut his hair? Eclipse it!",
#     "Why did the astronaut bring a ladder to space? Because they wanted to take their career to the next level!",
#     "What did the alien say to the garden? Take me to your weeder!",
#     "How do you throw a space party? You planet!",
#     "What do you call a lazy kangaroo? A pouch potato!",
#     "Why don't scientists trust atoms? Because they make up everything!",
#     "Why did the astronaut break up with the moon? He needed space!",
#     "What do you call a singing asteroid? A shooting star!",
#     "Why did the astronaut become a gardener? He wanted to explore space and thyme!",
#     "What did the astronaut say when he crashed into the moon? I Apollo-gize!",
#     "Why don't aliens eat clowns? Because they taste funny!",
#     "What did the astronaut say to the alien? 'Take me to your liter!'",
#     "How do you organize a space party? You planet!",
#     "Why did the sun go to school? To get a little brighter!",
#     "Why did the comet break up with the meteor? It couldn't handle the pressure!",
#     "Why did the astronaut break up with his girlfriend? Because he needed space!",
#     "Why did the cow go to space? It wanted to see the moooon!",
#     "What's an astronaut's favorite part of a computer? The space bar!",
#     "Why did the alien turn down the coffee? Because he was already 'orbit'!",
#     "Why did the asteroid go to school? To get a little meteor!",
#     ],
#     # Add more creative responses for each scenario
# }


# # Define patterns for greetings
# greeting_patterns = re.compile(r"\b(hello|hi|hey|greetings|howdy)\b", re.IGNORECASE)
# name_question_pattern = re.compile(r"\b(what('s| is) )?your name\b", re.IGNORECASE)
# how_are_you_pattern = re.compile(r"\b(how are you|how('s| is) it going)\b", re.IGNORECASE)
# planet_question_pattern = re.compile(r"\b(what('s| is) )?(your|the)? favorite planet\b", re.IGNORECASE)
# joke_pattern = re.compile(r"\b(tell me a joke|make me laugh)\b", re.IGNORECASE)


# @app.route("/llm/question", methods=["POST"])
# def question():
#     context = ""
#     question = request.json.get('question').strip().lower()

#     # Check for static responses first
#     if question in static_responses:
#         return jsonify(response=static_responses[question])

#     # Check for greeting patterns
#     if greeting_patterns.search(question):
#         return jsonify(response="Hello there! Ready for an out-of-this-world adventure?")

#     # Otherwise, generate a response using the LLM
#     full_prompt = template.format(context=context, question=question)
#     response = llm.invoke(full_prompt)
#     return jsonify(response=response)

# @app.route('/stream', methods=['POST'])
# def stream():
#     context = ""
#     question = request.json.get('question').strip().lower()

#     # Check for static responses first
#     if question in static_responses:
#         return Response(static_responses[question], mimetype='text/event-stream')

#     # Check for greeting patterns
#     if greeting_patterns.search(question):
#         return Response("Hello there! Ready for an out-of-this-world adventure?", mimetype='text/event-stream')

#     # Otherwise, generate a response using the LLM
#     full_prompt = template.format(context=context, question=question)
#     def generate():
#         for chunk in llm.stream(full_prompt):
#             yield chunk + ","

#     return Response(ssl.stream(generate()), mimetype='text/event-stream')

# def generate_tokens(question):
#     for chunks in llm.stream(question):
#         print(chunks)
#         yield chunks

# @app.route("/chat", methods=["POST"])
# def ask_ai():
#     def generate_json(question):
#         with app.app_context():  # Ensure we're within the application context
#             full_content = ""
#             context = ""
#             question = request.json.get('question').strip().lower()

#             # Check for static responses first
#             if question in static_responses:
#                 response = static_responses[question]
#                 print(f"Chat Static Response: {response}")
#                 yield json.dumps({
#                     "model": "llama2",
#                     "content": response,
#                     "done": True
#                 }).encode('utf-8')
#                 return

#             # Check for greeting patterns
#             if greeting_patterns.search(question):
#                 response = "Hello there! Ready for an out-of-this-world adventure?"
#                 print(f"Chat Greeting Response: {response}")
#                 yield json.dumps({
#                     "model": "llama2",
#                     "content": response,
#                     "done": True
#                 }).encode('utf-8')
#                 return

#             # Check for other scenarios
#             if name_question_pattern.search(question):
#                 response = random.choice(creative_responses["what is your name?"])
#                 print(f"Chat Name Question Response: {response}")
#                 yield json.dumps({
#                     "model": "llama2",
#                     "content": response,
#                     "done": True
#                 }).encode('utf-8')
#                 return
            
#             if how_are_you_pattern.search(question):
#                 response = random.choice(creative_responses["how are you?"])
#                 print(f"Chat How Are You Response: {response}")
#                 yield json.dumps({
#                     "model": "llama2",
#                     "content": response,
#                     "done": True
#                 }).encode('utf-8')
#                 return

#             if planet_question_pattern.search(question):
#                 response = random.choice(creative_responses["what's your favorite planet?"])
#                 print(f"Chat Planet Question Response: {response}")
#                 yield json.dumps({
#                     "model": "llama2",
#                     "content": response,
#                     "done": True
#                 }).encode('utf-8')
#                 return

#             if joke_pattern.search(question):
#                 response = random.choice(creative_responses["tell me a joke"])
#                 print(f"Chat Joke Response: {response}")
#                 yield json.dumps({
#                     "model": "llama2",
#                     "content": response,
#                     "done": True
#                 }).encode('utf-8')
#                 return

#             # Otherwise, generate a response using the LLM
#             full_prompt = template.format(context=context, question=question)
#             for token in generate_tokens(full_prompt):
#                 full_content += token
#                 json_data = {
#                     "model": "llama2",
#                     "content": token,
#                     "done": False
#                 }
#                 yield json.dumps(json_data).encode('utf-8')
#                 yield b'\n'  # Yield newline as bytes

#             # Once streaming is finished, yield one last JSON object with "done" set to True
#             json_data = {
#                 "model": "llama2",
#                 "full_content": full_content,
#                 "done": True
#             }
#             print(f"Final Chat Response: {full_content}")
#             yield json.dumps(json_data).encode('utf-8')

#     request_data = request.json
#     question = request_data.get("question")
#     return Response(stream_with_context(generate_json(question)), mimetype='application/json')

# if __name__ == '__main__':
#     app.run(debug=True, host='10.72.0.153')




from langchain_community.llms import Ollama
from langchain.prompts import PromptTemplate
from flask import Flask, render_template, request,jsonify
from flask import Flask, request, Response,stream_with_context
import json
import time
from flask_sse import sse
template = """You are a voice chat assistant in a child's AR application about space and precisely the solar system. The app contains information about planets in the solar system and their 3D animation in AR mode.
You are an astronaut and your name is "Astro". You are an expert in space exploration, particularly in astronautics and the solar system. Your goal is to provide comprehensive information on various aspects of space science and exploration. You are funny and helpful with children.
If you don't know the answer, just say that you don't know, don't try to make up an answer. Make your answers as short as possible. Answer in no more than 100 words. You are speaking to a child between the age of 5 and 14. Do not use emojis and words in between stars like *smiling* or *waving* describing the actions of the astro. Just pure conversational response."
{context}
Helpful Answer:"""

llm = Ollama(model="llama2")
# # print(llm.invoke("Tell me a joke"))


app = Flask(__name__)
app.static_folder = 'static'
 



# @app.route("/llm", methods=["POST"])
# def test():
#     return jsonify(test='hello')


# @app.route("/status", methods=["GET"])
# def status():
#     return jsonify(status="ok")

# # @app.route("/get")
# # def get_bot_response():
# #     # userText = request.args.get('msg')
# #     return jsonify(llm.invoke(str(question)))
# if __name__ == "__main__":
#     app.run(debug=True, host='10.72.0.153')



@app.route("/llm/question", methods=["POST"])
def question():
    # args = request.args
    # prompt = request.json
    # question = prompt["question"]
    print(request.json)
    context=""
    question = request.json.get('question')
    print(question)
    full_prompt = template.format(context=context, question=question)
    
    # if args.get("debug", default=False, type=bool):
    #     print("Your message is received...")
    #     print("Your question is: {}".format(question))
    response = llm.invoke(full_prompt)
    
    # if args.get("debug", default=False, type=bool):
    #     print("LLM response received")
    #     print(response)
    return jsonify(response=response)
@app.route('/stream', methods=['POST'])
def stream():
    context=""
    question = request.json.get('question')
    print(question)
    full_prompt = template.format(context=context, question=question)
    def generate():
        # Mock data generation, replace this with your llama2 invocation
        for chunk in llm.stream(full_prompt):
            #print(chunk, end="")
            yield chunk + ","
    
    return Response(sse.stream(generate()), mimetype='text/event-stream')

def generate_tokens(question):
    for chunks in llm.stream(question):
        print(chunks)
        yield chunks

@app.route("/chat", methods=["POST"])
def ask_ai():
    def generate_json(question):
        with app.app_context():  # Ensure we're within the application context
            full_content = ""
            context = ""  # Define your context or retrieve it from somewhere
            full_prompt = template.format(context=context, question=question)
            for token in generate_tokens(full_prompt):
                full_content += token + " "
                json_data = {
                    "model": "llama2",
                    "content": token,
                    "done": False
                }
                json_str = json.dumps(json_data)  # Convert JSON data to a string
                json_bytes = json_str.encode('utf-8')  # Encode JSON string to bytes
                yield json_bytes
                yield b'\n'  # Yield newline as bytes

            # Once streaming is finished, yield one last JSON object with "done" set to True
            json_data = {
                "model": "llama2",
                "full_content": full_content.strip(),
                "done": True
            }
            json_str = json.dumps(json_data)  # Convert JSON data to a string
            json_bytes = json_str.encode('utf-8')  # Encode JSON string to bytes
            yield json_bytes

    request_data = request.get_json()
    print(request_data)
    if isinstance(request_data, list):
        # Process each question in the list
        for item in request_data:
            if isinstance(item, dict):
                question = item.get("question")
                if question:
                    return Response(generate_json(question), content_type='application/json')
            else:
                return Response(json.dumps({"error": "Invalid item in list, expected dict"}), status=400, content_type='application/json')
    elif isinstance(request_data, dict):
        # Process the single question
        question = request_data.get("question")
        if question:
            return Response(stream_with_context(generate_json(question)), mimetype='application/json')
        else:
            return Response(json.dumps({"error": "Question not found in request"}), status=400, content_type='application/json')
    else:
        return Response(json.dumps({"error": "Invalid data format received"}), status=400, content_type='application/json')

    
    request_data = request.get_json()
    question = request_data[0].get("question")
    return Response(stream_with_context(generate_json(question)), mimetype='application/json')


if __name__ == '__main__':
    app.run(debug=True, host='192.168.1.194')
    

