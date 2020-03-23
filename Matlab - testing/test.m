r = zeros(216, 3);

n = 1;
for a = 1:6
    for b = 1:6
        for c = 1:6
            r(n, :) = [a,b,c];
            n = n + 1;
        end
    end
end

cnt = 0;
for i = 1:216
    temp = sort(r(i, :));
    if ( ~any(temp == 1) && ~any(temp == 2) && ~any(temp == 3) )
        temp
        cnt = cnt + 1;
    end
end

cnt