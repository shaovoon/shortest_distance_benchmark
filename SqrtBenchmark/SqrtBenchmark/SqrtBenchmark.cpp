#include <iostream>
#include <vector>
#include <random>
#include <iomanip>
#include <chrono>
#include <string>
#include <vector>
#include <map>
#include <unordered_map>
#include <cmath>

class timer
{
public:
	timer() = default;
	void start(const std::string& text_)
	{
		text = text_;
		begin = std::chrono::high_resolution_clock::now();
	}
	void stop()
	{
		auto end = std::chrono::high_resolution_clock::now();
		auto dur = end - begin;
		auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(dur).count();
		std::cout << std::setw(23) << text << " timing:" << std::setw(5) << ms << "ms" << std::endl;
	}

private:
	std::string text;
	std::chrono::high_resolution_clock::time_point begin;
};

class RandomNumGen
{
public:
	RandomNumGen(double min_value, double max_value)
		: gen(rd())
		, dis(min_value, max_value)
	{
	}
	static double GetRand()
	{
		static std::unique_ptr<RandomNumGen> randomNumGen;
		if (!randomNumGen)
			randomNumGen = std::unique_ptr<RandomNumGen>(new RandomNumGen(1.0, 1000.0));
		return randomNumGen->dis(randomNumGen->gen);
	}

	std::random_device rd;  //Will be used to obtain a seed for the random number engine
	std::mt19937 gen; //Standard mersenne_twister_engine seeded with rd()
	std::uniform_real_distribution<> dis;

};

struct Point
{
	Point(double x_, double y_) : x(x_), y(y_) {}
	double x;
	double y;
};

void InitVector(std::vector<Point>& vec, size_t total)
{
	vec.clear();
	for (size_t i = 0; i < total; ++i)
	{
		vec.push_back(Point(RandomNumGen::GetRand(), RandomNumGen::GetRand()));
	}
}

int main()
{
	const size_t MAX_LOOP = 1000000;
	std::vector<Point> vec;
	InitVector(vec, 1000);
	Point dest(RandomNumGen::GetRand(), RandomNumGen::GetRand());

	double shortest = std::numeric_limits<double>::max();
	size_t shortest_index = 0;
	double shortest2 = std::numeric_limits<double>::max();
	size_t shortest_index2 = 0;
	double shortest3 = std::numeric_limits<double>::max();
	double shortest_abs = std::numeric_limits<double>::max();
	size_t shortest_index3 = 0;

	timer stopwatch;
	stopwatch.start("With sqrt");
	for (size_t i = 0; i < MAX_LOOP; ++i)
	{
		shortest = std::numeric_limits<double>::max();
		shortest_index = 0;
		for (size_t j = 0; j < vec.size(); ++j)
		{
			const auto& pt = vec[j];
			double x = (pt.x - dest.x);
			double y = (pt.y - dest.y);
			x *= x;
			y *= y;
			double distance = sqrt(x + y);
			if (distance < shortest)
			{
				shortest = distance;
				shortest_index = j;
			}
		}
	}
	stopwatch.stop();

	stopwatch.start("Without sqrt");
	for (size_t i = 0; i < MAX_LOOP; ++i)
	{
		shortest2 = std::numeric_limits<double>::max();
		shortest_index2 = 0;
		for (size_t j = 0; j < vec.size(); ++j)
		{
			const auto& pt = vec[j];
			double x = (pt.x - dest.x);
			double y = (pt.y - dest.y);
			x *= x;
			y *= y;
			double distance = x + y;
			if (distance < shortest2)
			{
				shortest2 = distance;
				shortest_index2 = j;
			}
		}
		shortest2 = sqrt(shortest2);
	}
	stopwatch.stop();

	stopwatch.start("Bait out before square");
	for (size_t i = 0; i < MAX_LOOP; ++i)
	{
		shortest3 = std::numeric_limits<double>::max();
		shortest_index3 = 0;
		shortest_abs = std::numeric_limits<double>::max();
		for (size_t j = 0; j < vec.size(); ++j)
		{
			const auto& pt = vec[j];
			double x = fabs(pt.x - dest.x); // N.B.: abs!
			if (x > shortest_abs) continue; // bail out

			double y = fabs(pt.y - dest.y);
			if (y > shortest_abs) continue;

			double xq = x * x;
			double yq = y * y;
			double distance = xq + yq;
			if (distance < shortest3)
			{
				shortest3 = distance;
				shortest_abs = x + y;
				shortest_index3 = j;
			}
		}
		shortest3 = sqrt(shortest3);
	}
	stopwatch.stop();

	std::cout << "shortest: " << shortest << ", " << shortest2 << ", " << shortest3 << std::endl;
	std::cout << "shortest index: " << shortest_index << ", " << shortest_index2 << ", " << shortest_index3 << std::endl;

    std::cout << "Done!\n";
}
