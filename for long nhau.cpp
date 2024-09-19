#include<iostream>
#include<cmath>
#include <cstring>
int calculation(double a, double b, char operation);
int main()
{
	double x, y;
	char z;
	std::cout << "Please enter your X : ";
	std::cin >> x;
	std::cout << "\nPlease enter your Y : ";
	std::cin >> y;
	std::cout << "\nPlease choose the operation you wanted to solve (+,-,*,/) : ";
	std::cin >> z;
	calculation(x, y, z);
	std::cout<<"hello pedo enjoyer";
}
int calculation(double a, double b, char operation)
{
	double solution;
	switch (operation)
	{
	case '+':
		solution = a + b;
		std::cout << "Your answer is : " << solution;
		break;
	case'-':
		solution = a - b;
		std::cout << "Your answer is : " << solution;
		break;
	case'*':
		solution = a * b;
		std::cout << "Your answer is : " << solution;
		break;
	case'/':
		solution = a / b;
		std::cout << "Your answer is : " << solution;
		break;
	default:
		std::cout << "Your operation doesn't exist!";
		break;
	}
	return 0;
}
