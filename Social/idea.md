# Social


## Class

Class represents a population group.

The main output is the resource 'labour'.
    labour = labour_fraction * Population

### Innate

- Population
- Cohesion (sum of distance of all meme values)
- Growth Rate
- Health
- Labour Fraction
- Political Power

### Conflict

Classes have relationships to other classes.
Each conflict has properties.

- Class Shift (movement from one class to another)
- Solidarity (how easily memes spread between)
- Oppression (how much political power is transferred)

### Demographics

- Education
- Phenotype
- Domain

Demographics react slower to changes than memes, and are displayed as fractions.
Values of demographics can have associated memes. e.g. Phenotype-A 

### Memes

Memes are ideas that can gain 'adherence'. Adherance is comprised of two scalars adherence_min and adherence_max value.
Default value of meme is 0, positive number is support, negative opposition.
Naturally, both the max and min values will move towards the mean, but this can be modified by positive or negative pressures.
Positive pressures _only affect the movement of the max value_, and Negative pressures _only affect the movement of the min value_.

Memes can be muturally exclusive, inclusive or related in some way.