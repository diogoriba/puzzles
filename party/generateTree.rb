File.open('tree.json', 'w+') do |f|
  nodes = 1000
	nchildren = 3
  f.puts '{"tree":['
  nodes.times do |i|
		children = *(0..(nchildren-1))
		children.map! { |x| x + (nchildren*i) }
		children.map! { |x| (x >= nodes) ? 0 : x }
		children.delete(0)
    f.puts "{\"id\":#{i},\"coolness\":#{rand(100)},\"gender\":\"#{['m','f'].choice}\",\"children\":#{children.inspect}},"
  end
	f.seek -2, IO::SEEK_CUR #one for newline and one for ","
	f.puts "\n]}"
end
puts "Done!"
