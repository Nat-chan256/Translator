function sum(a, b)
{
            function innerFunction(c, d)
            {
                 let someVar = c * d - 1;
            }
	let s = a + b;
	return s;
}

let v1 = 3, v2;
let arr = [1, 2, 3];

for (var i = 0; i < 3; ++i)
{
   arr[i] = i * i;
}

while (i < 10)
{
    i += 2;
}

do
{
   i -= 1;
}
while (i > 0);

if (arr[2] % 2 == 0)
{
	arr[2] = sum(arr[0], arr[1]);
}
else
{
	arr[2] += 2;
}

if (arr[1] > arr[0])
{
	arr[1] -= arr[0];
}
        
var arr2 = [
    [1, 2, 3],
    [4, 5, 6]
];

arr2[0][ arr[0] + 1 ] = 0;
