# StringEncryption

Quando se deseja proteger o código de um projeto .NET você possui duas alternativas, manter o código oculto em um servidor ou ofuscação.
Esse projeto percorre o código MSIL de um software alvo, identifica strings, encripta, e salva o software modificado.

Por que não usar um algoritmo mais complexo para criptografar as strings?

É simples, descriptografar strings para um engenheiro reverso é algo simples e não podemos ocultar as strings descriptografadas na memória. Sendo assim é melhor optar por usar algoritmos simples que usem chaves para criptografia, como por exemplo, XOR, assim podemos manter uma certa segurança em nosso código e manter um ótimo desempenho.


Caso precise de ajuda para entender algo você pode entrar em contato comigo pelo meu servidor no Discord:
https://discord.com/invite/gU42BcPgGw
