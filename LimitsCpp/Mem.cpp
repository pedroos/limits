#include <stdio.h> 
#include <stdlib.h> 

typedef struct {
    int day;
    int month;
    int year;
} Type1;

int smain()
{
    void* coll = malloc(sizeof(int) + sizeof(Type1));
    if (coll == NULL) return 1;
    *((int*)coll) = 3;
    coll = ((int*)coll) + 1;
    
    Type1 type1;
    type1.day = 2;
    type1.month = 4;
    type1.year = 2008;

    *((Type1*)coll) = type1;

    coll = ((int*)coll) - 1;

    printf("%d\n", (int)*((int*)coll));
    coll = ((int*)coll) + 1;
    Type1* t1b = (Type1*)coll;
    printf("%d %d %d\n", t1b->day, t1b->month, t1b->year);

    return 0;
}