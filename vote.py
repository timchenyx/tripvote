import re

_vote_file_name = 'vote.csv'
# sample vote.csv
# name 1, abcdefgh
# name 2, hgfedcba

def normailze_vote(vote):
    # trim and remove dups
    s = set()
    vote = vote.strip().lower()

    def add_not_exists(e):
        exists = e in s
        if not exists:
            s.add(e)
        return not exists
    return [e for e in vote if add_not_exists(e)]

def read_votes():
    # sample:
    # [ ('name', ['a', 'b']), ('name', ...), ... ]
    path = _vote_file_name
    with open(path, 'r') as f:
        for l in f:
            l = l.rstrip('\n')
            r = re.split(',|\t', l)
            yield r[0], normailze_vote(r[1])

def init_count_map(options):
    # sample:
    # [('a', [0, 0, count]), ('b', [0, 0, count])]
    l = len(options) + 1

    def get_vote_count(i, o):
        if i < l - 1:
            return 0
        else:
            return options[o]

    return dict([(o, [get_vote_count(i, o) for i in range(l)]) for o in options])

def generate_count_map(options, votes):
    m = init_count_map(options)
    for name, vote in votes:  # pylint: disable=W0612
        for i, v in enumerate([v for v in vote if v in options]):
            m[v][i] += 1
    return m

def get_options(votes):
    o = dict()
    for name, vote in votes:  # pylint: disable=W0612
        for v in vote:
            if v in o:
                o[v] += 1
            else:
                o[v] = 1
    return o

def nice_print(result):
    for k, v in result:
        print(
            '{}:{}'.format(
                k,
                ','.join('{:>2}'.format(o) for o in v)
            )
        )

def main():
    votes = list(read_votes())
    options = get_options(votes)

    cnt = 0
    while(len(options) > 1):
        cnt += 1
        print('===== round {} ====='.format(cnt))
        result = generate_count_map(options, votes)
        result = sorted(result.items(), key=lambda e: e[1])

        del options[result[0][0]]

        print('result:')
        nice_print(result)
        print('remaining: {}'.format(options))

if __name__ == '__main__':
    main()
