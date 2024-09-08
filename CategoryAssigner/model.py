import torch
import torch.nn as nn
import numpy as np
from torchtext.vocab import GloVe
from torchtext.data.utils import get_tokenizer
from torch.utils.data import DataLoader

glove = GloVe(name='6B', dim=100)
tokenizer = get_tokenizer('basic_english')
categories = [
    'Grains', 'Diary', 'Fruit', 'Eggs', 'Meat', 'Fish', 'Vegetables', 
    'Fats/Oils', 'Nuts/Seeds', 'Sugar/Sugar products', 'Drink', 'Alcohol'
]


def text_to_embedding(txt : str) -> torch.Tensor:
    tokens = tokenizer(txt)

    embedding = [glove[token] for token in tokens if token in glove.stoi]

    if not embedding:
        return torch.zeros(1, glove.dim)
 
    return torch.stack(embedding)


class Net(nn.Module):
    def __init__(self, input_dim, output_dim):
        super(Net, self).__init__()
        self.input_layer = nn.Linear(input_dim, 64)
        self.relu = nn.ReLU()
        self.hidden_layer1 = nn.Linear(64, output_dim)

    def forward(self, x):
        x = torch.mean(x, dim=0)

        x = self.fc1(x)
        x = self.relu(x)
        x = self.fc2(x)

        return x
    

def train(model: nn.Module, funct: nn.Module, opt: torch.optim.Optimizer, loader: DataLoader, input_dim: int, output_dim: int, num_epochs: int = 10):
    model.train()

    for epoch in range(num_epochs):

        for txts, labels in loader:

            embeddings = [text_to_embedding(txt, glove, tokenizer) for txt in txts]
            inputs = torch.stack(embeddings)

            opt.zero_grad()

            outputs = model(inputs)
            loss = funct(outputs, labels)
            
            loss.backward()
            opt.step()


def resolve(model: nn.Module, txt: str) -> str:
    model.eval() 

    with torch.no_grad():
        embedding = text_to_embedding(txt)

        input_tensor = torch.stack(embedding)

        output = model(input_tensor)

        _, predicted = torch.max(output, 1)

    return categories[predicted.item()]


