# LUIS2Alexa
Utility to transform a LUIS export json into a .grammar file used to generate an Alexa Language Interaction Model. The intended usage is where you have a language understanding model you have developed in LUIS and you want to create an Alexa Language Interaction Model which is roughly equivalent in capabilities. It is *NOT* a magic Alexa skill generator.

This console app can be run like this from the command line:

```xml
C:\Dev> LUIS2Alexa *LUIS-export-file.json*
```

It produces one or more output files:
  - *LUIS-export-file-name*.grammar
  - one or more *entityName*.values files which are created from any closed list entities you have defined in your LUIS model

You must then take these input files and use those as inputs to the **Alexa Utterance Generator** which you can find at https://github.com/KayLerch/alexa-utterance-generator . Read the instructions there to find out how to use it.

**IMPORTANT** This utility isn't magic! It just helps you along the road. You will need to edit the resulting .grammar file to give better results when run through the utterance generator.
Things you may need to do:
  - Edit the grammar to flag up any optional words in utterances. For example: *BookingIntent: {|please} help me*
  - If you have used any built-in entities in your LUIS model, this utility cannot 'flag' those as slots in your .grammar file - you must edit the grammar yourself and substitute Amazon built-in slot types. For example: An utterance from the LUIS model of 'SearchHotels: can you show me hotels from los angeles?' becomes 'SearchHotels: can you show me hotels from {{AMAZON.US_CITY}}?'

See instructions at https://github.com/KayLerch/alexa-utterance-generator for more information on options in defining a grammar file.
